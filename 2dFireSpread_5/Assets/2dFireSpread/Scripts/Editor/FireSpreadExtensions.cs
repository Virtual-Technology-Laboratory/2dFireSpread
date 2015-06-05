using UnityEngine;
using UnityEditor;
using System.Collections;

using System.IO;

class FireSpreadExtensions
{

    [MenuItem("FireSpread/Save Map")]
    public static void SaveMap()
    {
        FireSpreadManager fireSpreadManager = Transform.FindObjectOfType<FireSpreadManager>();

        Texture2D texture = fireSpreadManager.EncodeToTexture();
        if (texture == null)
        {
            EditorUtility.DisplayDialog(
                "Select Texture",
                "You Must Select a Texture first!",
                "Ok");
            return;
        }

        var path = EditorUtility.SaveFilePanel(
                "Save texture as PNG",
                "",
                texture.name + ".png",
                "png");

        if (path.Length != 0)
        {
            var pngData = texture.EncodeToPNG();
            if (pngData != null)
                File.WriteAllBytes(path, pngData);
        }
    }

    [MenuItem("FireSpread/Load Map")]
    public static void LoadMap()
    {
        FireSpreadManager fireSpreadManager = Transform.FindObjectOfType<FireSpreadManager>();

        Texture2D texture = new Texture2D(fireSpreadManager.numWidth,
                                          fireSpreadManager.numHeight,
                                          TextureFormat.RGB24, false);

		var path = EditorUtility.OpenFilePanel(
				"Load from png",
				"",
				"png");
		if (path.Length != 0) {
			var www = new WWW("file:///" + path);
            www.LoadImageIntoTexture(texture);
            fireSpreadManager.LoadFromTexture(texture);
		}
    }
}