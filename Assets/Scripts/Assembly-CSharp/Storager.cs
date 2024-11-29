using System;
using System.Collections.Generic;
using UnityEngine;

public static class Storager
{
	private const bool useCryptoPlayerPrefs = true;

	private const bool _useSignedPreferences = true;

	private static bool iCloudAvailable;

	private static IDictionary<string, int> _keychainCache;

	private static IDictionary<string, string> _keychainStringCache;

	private static int _readCount;

	private static int _writeCount;

	public static int ReadCount
	{
		get
		{
			return _readCount;
		}
	}

	public static int WriteCount
	{
		get
		{
			return _writeCount;
		}
	}

	static Storager()
	{
		iCloudAvailable = false;
		_keychainCache = new Dictionary<string, int>();
		_keychainStringCache = new Dictionary<string, string>();
		int salt = 0x6B3C41E4 ^ Defs.SaltSeed;
		CryptoPlayerPrefs.setSalt(salt);
		CryptoPlayerPrefs.useRijndael(true);
		CryptoPlayerPrefs.useXor(true);
	}

	public static void Initialize(bool cloudAvailable)
	{
	}

	public static bool synchronize()
	{
		CryptoPlayerPrefs.Save();
		Defs.SignedPreferences.Save();
		return true;
	}

	public static bool hasKey(string key)
	{
		bool flag = CryptoPlayerPrefs.HasKey(key);
		/*string value;
		int result;
		if (key.Equals(Defs.Coins) && !flag && Defs.SignedPreferences.TryGetValue(Defs.Coins, out value) && Defs.SignedPreferences.Verify(Defs.Coins) && int.TryParse(value, out result))
		{
			setInt(Defs.Coins, Math.Max(0, result), false);
			return true;
		}*/
		return flag;
	}

	public static void removeObjectForKey(string key)
	{
		CryptoPlayerPrefs.DeleteKey(key);
		Defs.SignedPreferences.Remove(key);
	}

	public static void removeAll()
	{
		CryptoPlayerPrefs.DeleteAll();
		Defs.SignedPreferences.Clear();
	}

	public static void setInt(string key, int val, bool useICloud)
	{
		_writeCount++;
		CryptoPlayerPrefs.SetInt(key, val);
		/*if (key.Equals(Defs.Coins))
		{
			Defs.SignedPreferences.Add(Defs.Coins, val.ToString());
		}*/
	}

	public static int getInt(string key, bool useICloud)
	{
		_readCount++;
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefs.GetInt(key);
		}
		/*string value;
		int result;
		if (key.Equals(Defs.Coins) && Defs.SignedPreferences.TryGetValue(Defs.Coins, out value) && Defs.SignedPreferences.Verify(Defs.Coins) && int.TryParse(value, out result))
		{
			return result;
		}*/
		return 0;
	}

	public static void setString(string key, string val, bool useICloud)
	{
		if (Application.isEditor)
		{
			PlayerPrefs.SetString(key, val);
		}
		else
		{
			CryptoPlayerPrefs.SetString(key, val);
		}
	}

	public static string getString(string key, bool useICloud)
	{
		if (Application.isEditor)
		{
			return PlayerPrefs.GetString(key);
		}
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefs.GetString(key, string.Empty);
		}
		return string.Empty;
	}
}
