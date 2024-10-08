using System;
using System.IO;
using UnityEngine;

public class SkinsManager
{
	public static string _PathBase
	{
		get
		{
			return Application.persistentDataPath + "/GameData";
		}
	}

	private static void _WriteImageAtPathToGal(string pathToImage)
	{
		try
		{
		}
		catch (Exception ex)
		{
			Debug.Log("Exception in _ScreenshotWriteToAlbum: " + ex);
		}
	}

	public static void SaveTextureToGallery(Texture2D t, string nm)
	{
        //string pathToImage = Path.Combine(_PathBase, nm);
        //_WriteImageAtPathToGal(pathToImage);
        SaveTextureWithName(t, nm);

    }

    public static bool SaveTextureWithName(Texture2D t, string nm, bool writeToGallery = false)
    {
        string folderPath = _PathBase;  // Assuming _PathBase is a valid directory path
        string filePath = _PathBase + "/" +nm;

        // Ensure the directory exists before saving the file
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Check if the file exists, but you don't need to create it before writing to it
        if (File.Exists(filePath))
        {
            Debug.Log("File already exists, it will be overwritten.");
        }

        // Encode the texture to PNG
        byte[] bytes = t.EncodeToPNG();

        // Write the PNG to the file
        File.WriteAllBytes(filePath, bytes);

        // Optionally write to gallery
        if (writeToGallery)
        {
            _WriteImageAtPathToGal(filePath);
        }

        return true;
    }


    public static Texture2D TextureForName(string nm)
    {
        // Create a texture with a default size (you can change the size based on your needs)
        Texture2D texture2D = new Texture2D(64, 32);
        string filePath = _PathBase +"/"+ nm;

        try
        {
            // Check if the directory exists
            if (!Directory.Exists(_PathBase))
            {
                Debug.LogError("Directory does not exist: " + _PathBase);
                return texture2D;
            }

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Debug.LogError("File not found: " + filePath);
                return texture2D;
            }

            // Read the file and load the texture
            byte[] data = File.ReadAllBytes(filePath);
            texture2D.LoadImage(data); // Load the image data into the texture
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading texture: " + ex.Message);
            return null; // Return null if there is an error
        }

        return texture2D; // Return the texture if everything is successful
    }


    public static bool DeleteTexture(string nm)
	{
		try
		{
			File.Delete(Path.Combine(_PathBase, nm));
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		return true;
	}

	public static Texture currentMultiplayerSkin()
	{
		if (!File.Exists(Path.Combine(_PathBase, Defs.SkinBaseName + 1)))
		{
			return Resources.Load(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1")) as Texture;
		}
		string @string = PlayerPrefs.GetString(Defs.SkinNameMultiplayer, Defs.SkinBaseName + 0);
		return TextureForName(@string);
	}
}
