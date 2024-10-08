using UnityEngine;

public class InGameGUI : MonoBehaviour
{
	public delegate float GetFloatVAlue();

	public delegate string GetString();

	public delegate int GetIntVAlue();

	public GetFloatVAlue health;

	public GetFloatVAlue armor;

	public GetIntVAlue armorType;

	public GetString killsToMaxKills;

	public GetString timeLeft;

	public GameObject[] hearts = new GameObject[0];

	public GameObject[] armorShields = new GameObject[0];

	public GameObject[] goldenArmorShields = new GameObject[0];

	public GameObject[] crystalArmorShields = new GameObject[0];

	public GameObject elixir;

	public GameObject scoreLabel;

	public GameObject enemiesLabel;

	public GameObject timeLabel;

	public GameObject killsLabel;

	public GameObject scopeText;

	private void Start()
	{
		if (PlayerPrefs.GetInt("AddCoins", 0) == 1)
		{
			Invoke("GenerateMiganie", 1f);
			PlayerPrefs.SetInt("AddCoins", 0);
		}
	}

	private void GenerateMiganie()
	{
		CoinsMessage.FireCoinsAddedEvent();
	}

	private void Update()
	{
		for (int i = 0; i < Player_move_c.MaxPlayerHealth; i++)
		{
			hearts[i].SetActive((float)i < health());
		}
		for (int j = 0; j < Player_move_c.MaxPlayerHealth; j++)
		{
			armorShields[j].SetActive((float)j < armor() && armorType() == 0);
			goldenArmorShields[j].SetActive((float)j < armor() && armorType() == 1);
			crystalArmorShields[j].SetActive((float)j < armor() && armorType() == 2);
		}
	}

	public void SetScopeForWeapon(string weapon)
	{
		scopeText.SetActive(true);
		scopeText.GetComponent<UITexture>().mainTexture = Resources.Load(ResPath.Combine("Scopes", weapon)) as Texture;
	}

	public void ResetScope()
	{
		scopeText.GetComponent<UITexture>().mainTexture = null;
		scopeText.SetActive(false);
	}
}
