using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpreadManager : MonoBehaviour
{
    public enum InteractionMode { Fire, Firewall, Unburned }
    public InteractionMode interactionMode = InteractionMode.Fire;

    public enum LabelMode { None, Location }
    public LabelMode labelMode = LabelMode.Location;

    public GameObject HexPrefab;

    public int numHeight = 5;
    public int numWidth = 5;

    float xSpacing;
    float yOffset;
    float ySpacing;

    GameObject[,] hexCells;

    public float burnProbability = 0.2f;

    void Start()
    {
        DefineSpacingParameters();
        InitializeHexCells();
    }

    void DefineSpacingParameters()
    {
        xSpacing = HexPrefab.transform.localScale.x * 0.75f;
        yOffset = HexPrefab.transform.localScale.y * 0.5f;
        ySpacing = HexPrefab.transform.localScale.y;
    }

    void InitializeHexCells()
    {
        hexCells = new GameObject[numWidth, numHeight];
        foreach (Transform child in transform)
        {
            string[] s = child.name.Split()[1].Split(',');
            int i = System.Convert.ToInt32(s[0]);
            int j = System.Convert.ToInt32(s[1]);
            hexCells[i, j] = child.gameObject;
        }
    }

    public void Build()
    {
        DefineSpacingParameters();

        hexCells = new GameObject[numWidth, numHeight];

        for (int i = 0; i < numWidth; i++)
        {
            for (int j = 0; j < numHeight; j++)
            {
                GameObject g = Instantiate(HexPrefab);
                g.transform.parent = transform;
                g.name = string.Format("Cell {0},{1}", i, j);

                var xPos = i * xSpacing;
                var yPos = j * ySpacing + (i % 2 == 0 ? 0f : yOffset);
                g.transform.position = new Vector3(xPos, yPos, 0f);

                g.GetComponent<BurnState>()
                 .gridLocation = new Vector2(i, j);

                g.GetComponent<BurnState>()
                 .transform.FindChild("Text")
                 .GetComponent<TextMesh>()
                 .text = string.Format("{0},{1}", i, j);

                g.GetComponent<BurnState>()
                 .fireSpreadManager = this;
                hexCells[i, j] = g;
            }
        }

        // assign neighbors
        for (int i = 0; i < numWidth; i++)
        {
            for (int j = 0; j < numHeight; j++)
            {
                List<BurnState> neighbors = new List<BurnState>();

                if (i % 2 == 0)
                {
                    if (i > 0)
                        neighbors.Add(hexCells[i - 1, j].GetComponent<BurnState>());

                    if (j < numHeight - 1)
                        neighbors.Add(hexCells[i, j + 1].GetComponent<BurnState>());

                    if (i < numWidth - 1)
                        neighbors.Add(hexCells[i + 1, j].GetComponent<BurnState>());

                    if (i < numWidth - 1 && j > 0)
                        neighbors.Add(hexCells[i + 1, j - 1].GetComponent<BurnState>());

                    if (j > 0)
                        neighbors.Add(hexCells[i, j - 1].GetComponent<BurnState>());

                    if (i > 0 && j > 0)
                        neighbors.Add(hexCells[i - 1, j - 1].GetComponent<BurnState>());
                }
                else
                {
                    if (i > 0 && j < numHeight - 1)
                        neighbors.Add(hexCells[i - 1, j + 1].GetComponent<BurnState>());

                    if (j < numHeight - 1)
                        neighbors.Add(hexCells[i, j + 1].GetComponent<BurnState>());

                    if (i < numWidth - 1 && j < numHeight - 1)
                        neighbors.Add(hexCells[i + 1, j + 1].GetComponent<BurnState>());

                    if (i > 0)
                        neighbors.Add(hexCells[i - 1, j].GetComponent<BurnState>());

                    if (j > 0)
                        neighbors.Add(hexCells[i, j - 1].GetComponent<BurnState>());

                    if (i < numWidth - 1)
                        neighbors.Add(hexCells[i + 1, j].GetComponent<BurnState>());
                }

                hexCells[i, j].GetComponent<BurnState>().neighbors = neighbors;
            }
        }
    }

    public Texture2D EncodeToTexture()
    {
        Texture2D tex = new Texture2D(numWidth, numHeight, TextureFormat.RGB24, false);

        // loop through and build texture
        for (int i = 0; i < numWidth; i++)
        {
            for (int j = 0; j < numHeight; j++)
            {
                var burnState = hexCells[i, j].GetComponent<BurnState>().TheBurnState;

                var c = BurnState.burnStateColors[burnState];
                tex.SetPixel(i, j, c);
            }
        }

        return tex;
    }

    static Vector3 ColorToVector3(Color color)
    {
        return new Vector3(Mathf.RoundToInt(color.r * 255),
                           Mathf.RoundToInt(color.g * 255),
                           Mathf.RoundToInt(color.b * 255));
    }

    public void LoadFromTexture(Texture2D tex)
    {
        if (tex.width != numWidth || tex.height != numHeight)
            throw new System.Exception("Texture is not the correct size");

        var c2BurnState = new Dictionary<Vector3, BurnState.BurnStates>();
        foreach (var item in BurnState.burnStateColors)
        {   
            c2BurnState[ColorToVector3(item.Value)] = item.Key;
        }

        // loop through and build texture
        for (int i = 0; i < numWidth; i++)
        {
            for (int j = 0; j < numHeight; j++)
            {
                try
                {
                    var burnState = c2BurnState[ColorToVector3(tex.GetPixel(i, j))];
                    hexCells[i, j].GetComponent<BurnState>().TheBurnState = burnState;
                }
                catch
                {
                    Debug.Log(ColorToVector3(tex.GetPixel(i, j)));
                    Debug.Log(c2BurnState.Keys.ToString());
                    return;
                }
            }
        }
    }

    public void SetBurnProbability(float prob)
    {
        burnProbability = prob;
    }

    private void SetLabelsInactive()
    {
        foreach (Transform child in transform)
        {
            var text = child.FindChild("Text");
            if (text != null)
                text.gameObject.SetActive(false);
        }
    }

    private void SetLabelsActive()
    {
        foreach (Transform child in transform)
        {
            var text = child.FindChild("Text");
            if (text != null)
                text.gameObject.SetActive(true);
        }
    }

    public void SetInteractionMode(string interactionMode)
    {
        switch (interactionMode)
        {
            case "Fire":
                this.interactionMode = InteractionMode.Fire;
                break;
            case "Firewall":
                this.interactionMode = InteractionMode.Firewall;
                break;
            case "Unburned":
                this.interactionMode = InteractionMode.Unburned;
                break;
        }
    }

    public void SetInteractionMode(InteractionMode interactionMode)
    {
        this.interactionMode = interactionMode;
    }

    public void SetLabelMode(string labelMode)
    {
        switch (labelMode)
        {
            case "None":
                SetLabelMode(FireSpreadManager.LabelMode.None);
                break;
            case "Location":
                SetLabelMode(FireSpreadManager.LabelMode.Location);
                break;
        }
    }

    public void SetLabelMode(LabelMode labelMode)
    {
        this.labelMode = labelMode;

        switch(labelMode)
        {
            case LabelMode.None:
                SetLabelsInactive();
                break;
            case LabelMode.Location:
                SetLabelsActive();
                break;
        }
    }
}
