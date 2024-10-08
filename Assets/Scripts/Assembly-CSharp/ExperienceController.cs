using UnityEngine;

public class ExperienceController : MonoBehaviour
{
	public int currentLevel = 1;

	public int maxLevel = 8;

	public Texture2D[] marks;

	public int currentExperience;

	public int addCurrentExperience;

	public int[] maxExperienceLevels;

	public int[] addCoinsFromLevels;

	public bool isShowRanks = true;

	public bool isShowNextPlashka;

	public Texture2D exp_back;

	public Texture2D exp_frame;

	public Texture2D exp_green;

	public Texture2D exp_upgrade;

	public GUIStyle levelStyle;

	public GUIStyle currentExperenceStyle;

	public GUIStyle rankStyle;

	public GUIStyle okStyle;

	public Vector2 posRanks = Vector2.zero;

	private int oldCurrentExperience;

	private int oldCurrentLevel;

	public bool isShowAdd;

	private bool animAddExperience;

	private int stepAnim;

	public Texture2D nextPlashkaTexture;

	public AudioClip exp_1;

	public AudioClip exp_2;

	public AudioClip exp_3;

	public Font font1;

	public Font font2;

	public Font font3;

	private void Start()
	{
		font1 = levelStyle.font;
		font2 = rankStyle.font;
		font3 = currentExperenceStyle.font;
		for (int i = 1; i <= maxLevel; i++)
		{
			if (!Storager.hasKey("currentLevel" + i))
			{
				Storager.setInt("currentLevel" + i, (i == 1) ? 1 : 0, i == 1);
			}
		}
		if (!Storager.hasKey("currentExperience"))
		{
			Storager.setInt("currentExperience", 0, false);
		}
		for (int j = 1; j <= maxLevel; j++)
		{
			if (Storager.getInt("currentLevel" + j, true) == 1)
			{
				currentLevel = j;
				Storager.setInt("currentLevel" + currentLevel, 1, true);
			}
		}
		currentExperience = Storager.getInt("currentExperience", false);
		Object.DontDestroyOnLoad(base.gameObject);
		levelStyle.fontSize = Mathf.RoundToInt(16f * Defs.Coef);
		currentExperenceStyle.fontSize = Mathf.RoundToInt(16f * Defs.Coef);
		rankStyle.fontSize = Mathf.RoundToInt(16f * Defs.Coef);
	}

	public void addExperience(int experience)
	{
		if (currentLevel == maxLevel)
		{
			return;
		}
		isShowAdd = true;
		animAddExperience = true;
		stepAnim = 0;
		oldCurrentLevel = currentLevel;
		oldCurrentExperience = currentExperience;
		Invoke("AnimAddExperience", 0.15f);
		currentExperience += experience;
		Storager.setInt("currentExperience", currentExperience, false);
		if (currentLevel < maxLevel && currentExperience >= maxExperienceLevels[currentLevel])
		{
			currentExperience -= maxExperienceLevels[currentLevel];
			currentLevel++;
			Storager.setInt("currentLevel" + currentLevel, 1, true);
			Storager.setInt("currentExperience", currentExperience, false);
			if (!Storager.hasKey(Defs.Coins))
			{
				Storager.setInt(Defs.Coins, 0, false);
			}
			int @int = Storager.getInt(Defs.Coins, false);
			Storager.setInt(Defs.Coins, @int + addCoinsFromLevels[currentLevel - 1], false);
			CoinsMessage.FireCoinsAddedEvent();
		}
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			NGUITools.PlaySound(exp_1);
		}
	}

	private void AnimAddExperience()
	{
		stepAnim++;
		if (stepAnim < 9)
		{
			Invoke("AnimAddExperience", 0.15f);
			return;
		}
		animAddExperience = false;
		if (oldCurrentLevel < currentLevel)
		{
			isShowNextPlashka = true;
			nextPlashkaTexture = Resources.Load("Experience/rank_up" + oldCurrentLevel) as Texture2D;
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				NGUITools.PlaySound(exp_3);
			}
		}
		else
		{
			isShowAdd = false;
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				NGUITools.PlaySound(exp_2);
			}
		}
	}

	private void HideNextPlashka()
	{
		isShowNextPlashka = false;
		nextPlashkaTexture = null;
		isShowAdd = false;
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		if (coinsShop.thisScript.enabled)
		{
			return;
		}
		GUI.depth = -100;
		if (!isShowRanks)
		{
			return;
		}
		GUI.DrawTexture(new Rect(posRanks.x, posRanks.y, (float)exp_frame.width * Defs.Coef, (float)exp_frame.height * Defs.Coef), exp_back);
		if (animAddExperience && (stepAnim == 1 || stepAnim == 3 || stepAnim == 5 || stepAnim == 7))
		{
			float num = (float)currentExperience / (float)maxExperienceLevels[currentLevel];
			if (currentLevel > oldCurrentLevel)
			{
				num = 1f;
			}
			GUI.DrawTexture(new Rect(posRanks.x + 69f * Defs.Coef, posRanks.y + (float)(exp_frame.height - exp_green.height) * 0.5f * Defs.Coef, 180f * num * Defs.Coef, (float)exp_green.height * Defs.Coef), exp_upgrade);
		}
GUI.DrawTexture(
    new Rect(
        posRanks.x + 69f * Defs.Coef, // x
        posRanks.y + (float)(exp_frame.height - exp_green.height) * 0.5f * Defs.Coef, // y
        180f * (animAddExperience ? 
            ((currentLevel <= oldCurrentLevel) ? 
                ((float)oldCurrentExperience / (float)maxExperienceLevels[currentLevel]) 
                : 
                ((float)oldCurrentExperience / (float)maxExperienceLevels[currentLevel - 1])) 
            : 
            ((!isShowNextPlashka && currentLevel != maxLevel) ? 
                ((float)currentExperience / (float)maxExperienceLevels[currentLevel]) 
                : 
                1f)) * Defs.Coef,
        (float)exp_green.height * Defs.Coef 
    ),
    exp_green
);
		GUI.DrawTexture(new Rect(posRanks.x, posRanks.y, (float)exp_frame.width * Defs.Coef, (float)exp_frame.height * Defs.Coef), exp_frame);
		GUI.DrawTexture(new Rect(posRanks.x + 14f * Defs.Coef, posRanks.y + 14f * Defs.Coef, (float)marks[(!animAddExperience) ? currentLevel : oldCurrentLevel].width * Defs.Coef, (float)marks[(!animAddExperience) ? currentLevel : oldCurrentLevel].height * Defs.Coef), marks[(!animAddExperience) ? currentLevel : oldCurrentLevel]);
		GUI.Label(new Rect(posRanks.x + 185f * Defs.Coef, posRanks.y + 60f * Defs.Coef, 65f * Defs.Coef, 18f * Defs.Coef), "LEV." + ((!animAddExperience) ? currentLevel : oldCurrentLevel), levelStyle);
		GUI.Label(new Rect(posRanks.x + 73f * Defs.Coef, posRanks.y + 60f * Defs.Coef, 100f * Defs.Coef, 18f * Defs.Coef), (currentLevel != maxLevel) ? (((!animAddExperience) ? currentExperience : oldCurrentExperience) + "/" + maxExperienceLevels[(!animAddExperience) ? currentLevel : oldCurrentLevel]) : "FULL", currentExperenceStyle);
		GUI.Label(new Rect(posRanks.x + 12f * Defs.Coef, posRanks.y + 60f * Defs.Coef, 60f * Defs.Coef, 18f * Defs.Coef), "RANK", rankStyle);
		if (isShowNextPlashka)
		{
			Rect position = new Rect((float)Screen.width / 2f - 1366f * Defs.Coef / 2f, 0f, 1366f * Defs.Coef, 768f * Defs.Coef);
			GUI.DrawTexture(position, nextPlashkaTexture);
			if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)okStyle.normal.background.width * 0.5f * Defs.Coef, (float)Screen.height - (21f + (float)okStyle.normal.background.height) * Defs.Coef, (float)okStyle.normal.background.width * Defs.Coef, (float)okStyle.normal.background.height * Defs.Coef), string.Empty, okStyle))
			{
				HideNextPlashka();
			}
		}
	}
}
