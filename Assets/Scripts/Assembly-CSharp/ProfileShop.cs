using UnityEngine;

public class ProfileShop : MonoBehaviour
{
	public static string SceneToLoad;

	private void Start()
	{
		WearShop.sharedShop.loadShopCategories();
		WearShop.sharedShop.buyAction = delegate
		{
		};
		WearShop.sharedShop.resumeAction = delegate
		{
			WearShop.sharedShop.unloadShopCategories();
			WearShop.sharedShop.resumeAction = delegate
			{
			};
			WearShop.sharedShop.buyAction = delegate
			{
			};
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.interval = Defs.GoToProfileShopInterval;
			LoadConnectScene.textureToShow = Resources.Load("coinsFon") as Texture;
			LoadConnectScene.sceneToLoad = SceneToLoad;
			if (LoadConnectScene.sceneToLoad.Equals(Defs.MainMenuScene))
			{
				PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 1);
			}
			Application.LoadLevel(Defs.PromSceneName);
		};
	}

	private void OnGUI()
	{
		WearShop.sharedShop.ShowShop(true);
	}
}
