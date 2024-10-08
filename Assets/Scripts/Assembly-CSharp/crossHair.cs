using UnityEngine;

public class crossHair : MonoBehaviour
{
	public Texture2D crossHairTexture;

	private Rect crossHairPosition;

	private Pauser pauser;

	private Player_move_c playerMoveC;

	private PhotonView photonView;

	private void Start()
	{
		photonView = PhotonView.Get(this);
		if ((((PlayerPrefs.GetString("TypeConnect").Equals("local") && base.GetComponent<NetworkView>().isMine) || (PlayerPrefs.GetString("TypeConnect").Equals("inet") && photonView.isMine)) && PlayerPrefs.GetInt("MultyPlayer") == 1) || PlayerPrefs.GetInt("MultyPlayer") != 1)
		{
			crossHairPosition = new Rect((Screen.width - crossHairTexture.width * Screen.height / 640) / 2, (Screen.height - crossHairTexture.height * Screen.height / 640) / 2, crossHairTexture.width * Screen.height / 640, crossHairTexture.height * Screen.height / 640);
			pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
			playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		}
	}

	private void OnGUI()
	{
		if (((((PlayerPrefs.GetString("TypeConnect").Equals("local") && base.GetComponent<NetworkView>().isMine) || (PlayerPrefs.GetString("TypeConnect").Equals("inet") && photonView.isMine)) && PlayerPrefs.GetInt("MultyPlayer") == 1) || PlayerPrefs.GetInt("MultyPlayer") != 1) && !pauser.paused)
		{
			GUI.DrawTexture(crossHairPosition, crossHairTexture);
		}
	}
}
