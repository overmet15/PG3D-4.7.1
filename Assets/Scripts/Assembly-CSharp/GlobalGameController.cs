using System.Collections.Generic;
using UnityEngine;

public class GlobalGameController
{
	public static List<int> survScoreThresh;

	public static int curThr;

	public static int thrStep;

	public static Font fontHolder;

	public static readonly int NumOfLevels;

	private static int _currentLevel;

	private static int _allLevelsCompleted;

	private static int score;

	public static bool showTableMyPlayer;

	public static bool isFullVersion;

	public static Vector3 posMyPlayer;

	public static Quaternion rotMyPlayer;

	public static float healthMyPlayer;

	public static int numOfCompletedLevels;

	public static int totalNumOfCompletedLevels;

	public static int countKillsBlue;

	public static int countKillsRed;

	public static int coinsBase;

	public static int coinsBaseAdding;

	public static int levelsToGetCoins;

	public static readonly string AppVersion;

	public static int currentLevel
	{
		get
		{
			return _currentLevel;
		}
		set
		{
			_currentLevel = value;
		}
	}

	public static int AllLevelsCompleted
	{
		get
		{
			return _allLevelsCompleted;
		}
		set
		{
			_allLevelsCompleted = value;
		}
	}

	public static int ZombiesInWave
	{
		get
		{
			return 4;
		}
	}

	public static int EnemiesToKill
	{
		get
		{
			return (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0) ? 30 : 5;
		}
	}

	public static int Score
	{
		get
		{
			return score;
		}
		set
		{
			score = value;
			if (Defs.IsSurvival && score >= curThr)
			{
				int @int = Storager.getInt(Defs.Coins, false);
				Storager.setInt(Defs.Coins, @int + curThr / thrStep, false);
				CoinsMessage.FireCoinsAddedEvent();
				curThr += thrStep;
			}
		}
	}

	public static int SimultaneousEnemiesOnLevelConstraint
	{
		get
		{
			return 20;
		}
	}

	static GlobalGameController()
	{
		survScoreThresh = new List<int>();
		thrStep = 10000;
		fontHolder = null;
		NumOfLevels = 11;
		_currentLevel = -1;
		_allLevelsCompleted = 0;
		score = 0;
		showTableMyPlayer = false;
		isFullVersion = true;
		numOfCompletedLevels = 0;
		totalNumOfCompletedLevels = 0;
		countKillsBlue = 0;
		countKillsRed = 0;
		coinsBase = 1;
		coinsBaseAdding = 0;
		levelsToGetCoins = 1;
		AppVersion = "4.7.1";
	}

	private static void Swap(IList<int> list, int indexA, int indexB)
	{
		int value = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = value;
	}

	public static void ResetParameters()
	{
		AllLevelsCompleted = 0;
		numOfCompletedLevels = -1;
		totalNumOfCompletedLevels = -1;
	}
}
