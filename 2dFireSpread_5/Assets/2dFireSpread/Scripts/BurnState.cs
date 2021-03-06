﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BurnState : MonoBehaviour
{
    public enum BurnStates { Burned, Unburned, Burning, Firewall };
    BurnStates burnState;
    public BurnStates TheBurnState;

    public FireSpreadManager fireSpreadManager;

    public Vector2 gridLocation;

    public List<BurnState> neighbors;
    MeshRenderer meshRenderer;

    public static Color BurnedColor =   new Color(150f / 255f,  56f / 255f,  30f / 255f);
    public static Color UnburnedColor = new Color(116f / 255f, 193f / 255f, 62f / 255f);
    public static Color BurningColor = new Color(234f / 255f, 63f / 255f, 0f / 255f);
    public static Color FirewallColor = new Color(18f / 255f, 200f / 255f, 248f / 255f);
    public static Dictionary<BurnStates, Color> burnStateColors;

    // Use this for initialization
    void Start()
    {
        burnState = TheBurnState = BurnStates.Unburned;
        meshRenderer = GetComponent<MeshRenderer>();
        burnStateColors = new Dictionary<BurnStates, Color>();
        burnStateColors.Add(BurnStates.Burned,   BurnedColor);
        burnStateColors.Add(BurnStates.Unburned, UnburnedColor);
        burnStateColors.Add(BurnStates.Burning,  BurningColor);
        burnStateColors.Add(BurnStates.Firewall, FirewallColor);
    }

    void Update()
    {
        // Need to set the actually burn state after all the agents have a chance 
        // to run through the fixed update logic. So we check to see if the burnState
        // needs to be updated here.
        //
        // This is essentially a really inefficient FixedLateUpdate
        if (TheBurnState != burnState)
        {
            burnState = TheBurnState;
            meshRenderer.material.SetColor("_EmissionColor", burnStateColors[burnState]);
        }
    }

    public void OnMouseUp()
    {
        if (fireSpreadManager.interactionMode == FireSpreadManager.InteractionMode.Fire)
            TheBurnState = BurnStates.Burning;
        else if (fireSpreadManager.interactionMode == FireSpreadManager.InteractionMode.Firewall)
            TheBurnState = BurnStates.Firewall;
        else if (fireSpreadManager.interactionMode == FireSpreadManager.InteractionMode.Unburned)
            TheBurnState = BurnStates.Unburned;
    }


    void FixedUpdate()
    {
        //if (gridLocation.x == 0 && gridLocation.y == 0)
        //    Debug.Log("update");

        if (burnState != BurnStates.Firewall)
        {
            if (burnState == BurnStates.Unburned)
            {
                if (AnyNeighborsBurning())
                    if (Random.Range(0f, 1f) < fireSpreadManager.burnProbability)
                        TheBurnState = BurnStates.Burning;
            }
            else if (burnState == BurnStates.Burning)
                TheBurnState = BurnStates.Burned;
        }

    }

    bool AnyNeighborsBurning()
    {
        foreach (var neighbor in neighbors)
            if (neighbor.burnState == BurnStates.Burning)
                return true;

        return false;
    }

    int NumNeighborsBurning()
    {
        int count = 0;
        foreach (var neighbor in neighbors)
            if (neighbor.burnState == BurnStates.Burning)
                count++;

        return count;
    }
}
