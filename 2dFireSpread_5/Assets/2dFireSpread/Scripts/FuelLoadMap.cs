using UnityEngine;
using System.Collections;

public class FuelLoadMap : MonoBehaviour
{
    string fuelmap_path = @"Maps/fuelload";
    void Start()
    {
        Texture2D tex = Resources.Load<Texture2D>(fuelmap_path);
        float[,] fuelload = new float[tex.width, tex.height];
        
        for (int i = 0; i < tex.width; i++)
            for (int j = 0; j < tex.height; j++)
                fuelload[i, j] = tex.GetPixel(i, j).r;
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
