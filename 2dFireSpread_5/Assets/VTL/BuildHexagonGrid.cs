using UnityEngine;
using System.Collections;

public class BuildHexagonGrid : MonoBehaviour
{
    public GameObject HexPrefab;

    public int numHeight = 5;
    public int numWidth = 5;


    float xSpacing;
    float yOffset;
    float ySpacing;

    void Start()
    {
        xSpacing = HexPrefab.transform.localScale.x * 0.75f;
        yOffset = HexPrefab.transform.localScale.y * 0.5f;
        ySpacing = HexPrefab.transform.localScale.y;
        Build();
    }

    void Build()
    {
        for (int i = 0; i < numWidth; i++)
        {
            for (int j = 0; j < numHeight; j++)
            {
                GameObject g = Instantiate(HexPrefab);
                g.transform.parent = transform;
                g.transform.position = new Vector3(
                    i * xSpacing,
                    j * ySpacing + (i % 2 == 0 ? 0f: yOffset),
                    0f);
            }
        }
    }
}
