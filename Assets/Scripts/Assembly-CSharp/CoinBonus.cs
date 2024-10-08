using System.Collections.Generic;
using UnityEngine;

public class CoinBonus : MonoBehaviour
{
	public GameObject player;

	public AudioClip CoinItemUpAudioClip;

	private Player_move_c test;

	public void SetPlayer()
	{
		test = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (test == null || player == null || !(Vector3.Distance(base.transform.position, player.transform.position) < 1.5f))
		{
			return;
		}
		if (!Defs.IsTraining)
		{
			int @int = Storager.getInt(Defs.Coins, false);
			Storager.setInt(Defs.Coins, @int + 1, false);
		}
		CoinsMessage.FireCoinsAddedEvent();
		if (!Defs.IsSurvival)
		{
			string[] array = Storager.getString(Defs.LevelsWhereGetCoinS, false).Split("#"[0]);
			List<string> list = new List<string>();
			string[] array2 = array;
			foreach (string item in array2)
			{
				list.Add(item);
			}
			if (!list.Contains(Application.loadedLevelName))
			{
				list.Add(Application.loadedLevelName);
				Storager.setString(Defs.LevelsWhereGetCoinS, string.Join("#"[0].ToString(), list.ToArray()), false);
			}
		}
		if (Defs.IsTraining)
		{
			TrainingController.isNextStep = TrainingController.stepTrainingList["GetTheCoin"];
		}
		Object.Destroy(base.gameObject);
	}
}
