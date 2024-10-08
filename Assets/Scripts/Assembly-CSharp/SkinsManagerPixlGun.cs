using System.Collections;
using UnityEngine;

public class SkinsManagerPixlGun : MonoBehaviour
{
    public Hashtable skins = new Hashtable();

    private void OnLevelWasLoaded(int idx)
    {
        if (skins.Count > 0)
        {
            skins.Clear();
        }

        string path;

        if (PlayerPrefs.GetInt("MultyPlayer", 0) == 1 && PlayerPrefs.GetInt("COOP", 0) == 1 && PlayerPrefs.GetInt("company", 0) == 0)
        {
            path = "EnemySkins/COOP/";
        }
        else
        {
            if (PlayerPrefs.GetInt("MultyPlayer", 0) != 0 || PlayerPrefs.GetInt("COOP", 0) != 0 || PlayerPrefs.GetInt("company", 0) != 0)
            {
                return;
            }

            path = !Defs.IsSurvival ? 
                   ("EnemySkins/Level" + (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0 ? CurrentCampaignGame.currentLevel.ToString() : "3")) : 
                   Defs.SurvSkinsPath;
        }

        Texture[] array = Resources.LoadAll<Texture>(path);
        foreach (Texture tex in array) skins[tex.name] = tex;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}