using System.Collections.Generic;
using UnityEngine;

public class TrainingController : MonoBehaviour
{
	public static int stepTraining;

	public static Dictionary<string, int> stepTrainingList;

	public static int isNextStep;

	private Rect animTextureRect;

	private GameObject coinsPrefab;

	public Texture2D[] animTextures;

	private static int stepAnim;

	private static int maxStepAnim;

	private static bool isCanceled;

	public static bool isPressSkip;

	private float speedAnim;

	private int setNextStepInd;

	private Texture2D shop;

	private Texture2D shop_n;

	private bool isAnimShop;

	private Player_move_c player_move_c;

	public static int oldStepTraning;

	static TrainingController()
	{
		stepTraining = -1;
		stepTrainingList = new Dictionary<string, int>();
		isNextStep = 0;
		int num = 1;
		stepTrainingList.Add("SwipeToRotate", num++);
		stepTrainingList.Add("TapToMove", num++);
		stepTrainingList.Add("GetTheGun", num++);
		stepTrainingList.Add("WellDone", num++);
		stepTrainingList.Add("GetTheCoin", num++);
		stepTrainingList.Add("WellDoneCoin", num++);
		stepTrainingList.Add("InterTheShop", num++);
		stepTrainingList.Add("Shop", num++);
		stepTrainingList.Add("TapToShoot", num++);
		stepTrainingList.Add("SwipeWeapon", num++);
		stepTrainingList.Add("KillZombi", num++);
		stepTrainingList.Add("GoToPortal", num++);
	}

	public static void SkipTraining()
	{
		oldStepTraning = stepTraining;
		stepTraining = 0;
		isPressSkip = true;
		isCanceled = true;
	}

	public static void CancelSkipTraining()
	{
		isCanceled = false;
		isPressSkip = false;
		stepTraining = oldStepTraning;
		if (stepAnim == 0)
		{
			GameObject.FindGameObjectWithTag("TrainingController").GetComponent<TrainingController>().FirstStep();
		}
		else
		{
			GameObject.FindGameObjectWithTag("TrainingController").GetComponent<TrainingController>().NextStepAnim();
		}
	}

	private void Start()
	{
		animTextures = new Texture2D[3];
		stepTraining = 0;
		StartNextStepTraning();
		coinsPrefab = GameObject.FindGameObjectWithTag("CoinBonus");
		if (coinsPrefab != null)
		{
			coinsPrefab.SetActive(false);
		}
	}

	public void StartNextStepTraning()
	{
		stepTraining++;
		Vector2 vector = Vector2.zero;
		if (stepTraining == stepTrainingList["SwipeToRotate"])
		{
			isCanceled = true;
			maxStepAnim = 13;
			speedAnim = 0.5f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_rotate_0") as Texture2D;
			animTextures[1] = Resources.Load("Training/ob_rotate_1") as Texture2D;
			vector = new Vector2((float)Screen.width - (float)animTextures[0].width * Defs.Coef, (float)Screen.height - (float)animTextures[0].height * Defs.Coef);
		}
		if (stepTraining == stepTrainingList["TapToMove"])
		{
			isCanceled = true;
			maxStepAnim = 19;
			speedAnim = 0.5f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_move_0") as Texture2D;
			animTextures[1] = Resources.Load("Training/ob_move_1") as Texture2D;
			animTextures[2] = Resources.Load("Training/ob_move_2") as Texture2D;
			vector = new Vector2(0f, (float)Screen.height - (float)animTextures[0].height * Defs.Coef);
		}
		if (stepTraining == stepTrainingList["GetTheGun"])
		{
			isCanceled = true;
			maxStepAnim = 2;
			speedAnim = 0.2f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_pick_gun") as Texture2D;
			vector = new Vector2((float)Screen.width * 0.5f - (float)animTextures[0].width * 0.5f * Defs.Coef, (float)Screen.height * 0.25f - (float)animTextures[0].height * 0.5f * Defs.Coef);
			GameObject wp = Resources.Load("Weapons/Weapon2") as GameObject;
			Vector3 pos = new Vector3(1.05f, 0.25f, -6.79f);
			BonusCreator._CreateBonus(wp, pos);
		}
		if (stepTraining == stepTrainingList["WellDone"] || stepTraining == stepTrainingList["WellDoneCoin"])
		{
			isCanceled = true;
			maxStepAnim = 2;
			speedAnim = 3f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_well_done") as Texture2D;
			vector = new Vector2((float)Screen.width * 0.5f - (float)animTextures[0].width * 0.5f * Defs.Coef, (float)Screen.height * 0.25f - (float)animTextures[0].height * 0.5f * Defs.Coef);
		}
		if (stepTraining == stepTrainingList["GetTheCoin"])
		{
			if (coinsPrefab != null)
			{
				coinsPrefab.SetActive(true);
				coinsPrefab.GetComponent<CoinBonus>().SetPlayer();
			}
			isCanceled = true;
			maxStepAnim = 2;
			speedAnim = 3f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_jumptext") as Texture2D;
			vector = new Vector2((float)Screen.width * 0.5f - (float)animTextures[0].width * 0.5f * Defs.Coef, (float)Screen.height * 0.25f - (float)animTextures[0].height * 0.5f * Defs.Coef);
		}
		if (stepTraining == stepTrainingList["InterTheShop"])
		{
			WindowsMouseManager.Instance.SetMouseLock(false);
			player_move_c = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
			shop = player_move_c.buyStyle.normal.background;
			shop_n = player_move_c.buyStyle.active.background;
			isAnimShop = false;
			Invoke("AnimShop", 0.3f);
			isCanceled = true;
			maxStepAnim = 13;
			speedAnim = 0.3f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_shop_0") as Texture2D;
			animTextures[1] = Resources.Load("Training/ob_shop_1") as Texture2D;
			vector = new Vector2((float)Screen.width * 0.5f - (float)animTextures[0].width * 0.5f * Defs.Coef, 0f);
		}
		if (stepTraining == stepTrainingList["TapToShoot"])
		{
            WindowsMouseManager.Instance.SetMouseLock(true);
            isCanceled = true;
			maxStepAnim = 2;
			speedAnim = 3f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_shoot") as Texture2D;
			vector = new Vector2((float)Screen.width * 0.5f - (float)animTextures[0].width * 0.5f * Defs.Coef, (float)Screen.height * 0.3f - (float)animTextures[0].height * 0.5f * Defs.Coef);
		}
		if (stepTraining == stepTrainingList["SwipeWeapon"])
		{
			isCanceled = true;
			maxStepAnim = 13;
			speedAnim = 0.3f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_weapons_0") as Texture2D;
			animTextures[1] = Resources.Load("Training/ob_weapons_1") as Texture2D;
			vector = new Vector2((float)Screen.width - (float)animTextures[0].width * Defs.Coef, 0f);
		}
		if (stepTraining == stepTrainingList["KillZombi"])
		{
			GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<ZombieCreator>().BeganCreateEnemies();
			isCanceled = true;
			maxStepAnim = 2;
			speedAnim = 3f;
			stepAnim = 0;
			animTextures[0] = Resources.Load("Training/ob_killall") as Texture2D;
			vector = new Vector2((float)Screen.width * 0.5f - (float)animTextures[0].width * 0.5f * Defs.Coef, (float)Screen.height * 0.25f - (float)animTextures[0].height * 0.5f * Defs.Coef);
		}
		animTextureRect = new Rect(vector.x, vector.y, (float)animTextures[0].width * Defs.Coef, (float)animTextures[0].height * Defs.Coef);
		Invoke("FirstStep", 1f);
	}

	private void AnimShop()
	{
		isAnimShop = !isAnimShop;
		if (isAnimShop && stepTraining == stepTrainingList["InterTheShop"])
		{
			player_move_c.buyStyle.normal.background = shop_n;
			player_move_c.buyStyle.active.background = shop;
		}
		else
		{
			player_move_c.buyStyle.normal.background = shop;
			player_move_c.buyStyle.active.background = shop_n;
		}
		if (stepTraining == stepTrainingList["InterTheShop"])
		{
			Invoke("AnimShop", 0.3f);
		}
	}

	private void FirstStep()
	{
		isCanceled = false;
		stepAnim = 0;
		NextStepAnim();
	}

	private void NextStepAnim()
	{
		if (!isCanceled)
		{
			stepAnim++;
			if (stepTraining == stepTrainingList["WellDone"] && stepAnim >= maxStepAnim)
			{
				isNextStep = stepTrainingList["WellDone"];
			}
			else if (stepTraining == stepTrainingList["WellDoneCoin"] && stepAnim >= maxStepAnim)
			{
				isNextStep = stepTrainingList["WellDoneCoin"];
			}
			else
			{
				Invoke("NextStepAnim", speedAnim);
			}
		}
	}

	private void Update()
	{
		if (coinsPrefab == null && stepTraining < stepTrainingList["GetTheCoin"])
		{
			coinsPrefab = GameObject.FindGameObjectWithTag("CoinBonus");
			if (coinsPrefab != null)
			{
				coinsPrefab.SetActive(false);
			}
		}
		if (isNextStep > setNextStepInd)
		{
			setNextStepInd = isNextStep;
			if (stepTraining == stepTrainingList["SwipeToRotate"] || stepTraining == stepTrainingList["TapToMove"])
			{
				Invoke("StartNextStepTraning", 1.5f);
			}
			else if (stepTraining == stepTrainingList["TapToShoot"])
			{
				Invoke("StartNextStepTraning", 3f);
			}
			else
			{
				StartNextStepTraning();
			}
		}
	}

	private void OnGUI()
	{
		if (stepTraining == stepTrainingList["SwipeToRotate"] || stepTraining == stepTrainingList["InterTheShop"] || stepTraining == stepTrainingList["SwipeWeapon"])
		{
			if (stepAnim / 2 * 2 - stepAnim == -1)
			{
				GUI.DrawTexture(animTextureRect, animTextures[0]);
			}
			if (stepAnim != 0 && stepAnim / 2 * 2 - stepAnim == 0)
			{
				GUI.DrawTexture(animTextureRect, animTextures[1]);
			}
		}
		if (stepTraining == stepTrainingList["TapToMove"])
		{
			if (stepAnim / 3 * 3 - stepAnim == -1)
			{
				GUI.DrawTexture(animTextureRect, animTextures[0]);
			}
			if (stepAnim / 3 * 3 - stepAnim == -2)
			{
				GUI.DrawTexture(animTextureRect, animTextures[1]);
			}
			if (stepAnim != 0 && stepAnim / 3 * 3 - stepAnim == 0)
			{
				GUI.DrawTexture(animTextureRect, animTextures[2]);
			}
		}
		if ((stepTraining == stepTrainingList["WellDone"] || stepTraining == stepTrainingList["WellDoneCoin"] || stepTraining == stepTrainingList["TapToShoot"] || stepTraining == stepTrainingList["GetTheGun"] || stepTraining == stepTrainingList["GetTheCoin"] || stepTraining == stepTrainingList["KillZombi"]) && stepAnim > 0)
		{
			GUI.DrawTexture(animTextureRect, animTextures[0]);
		}
	}
}
