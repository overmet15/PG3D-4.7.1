using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class PingCloudRegions : MonoBehaviour
{
	private const string playerPrefsKey = "PUNCloudBestRegion";

	public static CloudServerRegion closestRegion = CloudServerRegion.US;

	public static PingCloudRegions SP;

	private bool isPinging;

	private int lowestRegionAverage = -1;

	private void Awake()
	{
		SP = this;
		if (PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty) != string.Empty)
		{
			string @string = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			closestRegion = (CloudServerRegion)(int)Enum.Parse(typeof(CloudServerRegion), @string, true);
		}
		else
		{
			StartCoroutine(PingAllRegions());
		}
	}

	public static void OverrideRegion(CloudServerRegion region)
	{
		SetRegion(region);
	}

	public static void RefreshCloudServerRating()
	{
		if (SP != null)
		{
			SP.StartCoroutine(SP.PingAllRegions());
		}
	}

	public static void ConnectToBestRegion(string gameVersion)
	{
		SP.StartCoroutine(SP.ConnectToBestRegionInternal(gameVersion));
	}

	public IEnumerator PingAllRegions()
	{
		ServerSettings settings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));
		if (settings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			yield break;
		}
		isPinging = true;
		foreach (int region in Enum.GetValues(typeof(CloudServerRegion)))
		{
			yield return StartCoroutine(PingRegion((CloudServerRegion)region));
		}
		isPinging = false;
	}

	private IEnumerator PingRegion(CloudServerRegion region)
	{
		string hostname = ServerSettings.FindServerAddressForRegion(region);
		string regionIp = ResolveHost(hostname);
		if (string.IsNullOrEmpty(regionIp))
		{
			Debug.LogError("Could not resolve host: " + hostname);
			yield break;
		}
		int averagePing = 0;
		int tries = 3;
		int skipped = 0;
		float timeout = 0.5f;
		for (int i = 0; i < tries; i++)
		{
			float startTime = Time.time;
			Ping ping = new Ping(regionIp);
			while (!ping.isDone && Time.time < startTime + timeout)
			{
				yield return 0;
			}
			if (ping.time == -1)
			{
				if (skipped > 5)
				{
					averagePing += (int)(timeout * 1000f) * tries;
					break;
				}
				i--;
				skipped++;
			}
			else
			{
				averagePing += ping.time;
			}
		}
		int regionAverage = averagePing / tries;
		if (regionAverage < lowestRegionAverage || lowestRegionAverage == -1)
		{
			lowestRegionAverage = regionAverage;
			SetRegion(region);
		}
	}

	private static void SetRegion(CloudServerRegion region)
	{
		closestRegion = region;
		PlayerPrefs.SetString("PUNCloudBestRegion", region.ToString());
	}

	private IEnumerator ConnectToBestRegionInternal(string gameVersion)
	{
		while (isPinging)
		{
			yield return 0;
		}
		ServerSettings settings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));
		if (settings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
		else
		{
			PhotonNetwork.Connect(ServerSettings.FindServerAddressForRegion(closestRegion), settings.ServerPort, settings.AppID, gameVersion);
		}
	}

	public static string ResolveHost(string hostString)
	{
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostString);
			foreach (IPAddress iPAddress in hostAddresses)
			{
				if (iPAddress != null && iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					return iPAddress.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Exception caught! " + ex.Source + " Message: " + ex.Message);
		}
		return string.Empty;
	}
}
