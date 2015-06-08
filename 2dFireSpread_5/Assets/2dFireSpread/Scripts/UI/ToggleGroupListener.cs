using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleGroupListener : MonoBehaviour
{
    // Adapted from https://github.com/AyARL/UnityGUIExamples/blob/master/ToggleGroup/Assets/ToggleGroupControl.cs
    ToggleGroup toggleGroup = null;
    internal Toggle[] toggles = null;

    [SerializeField]
    public string result = "";

    // Use this for initialization
    void Awake()
    {
        toggleGroup = gameObject.GetComponent<ToggleGroup>();
        toggles = gameObject.GetComponentsInChildren<Toggle>();

        // Register all toggles in the group
        foreach (Toggle toggle in toggles)
        {
            string name = toggle.name;
            toggle.onValueChanged.AddListener(
                (value) => ToggleStateChanged(name, value));

            // Assign group to the toggle to enable group logic
            toggle.group = toggleGroup;
        }
    }

    public virtual void ToggleStateChanged(string name, bool state)
    {
        // If the toggle was enabled notify the group
        if (state == true)
        {
            result = name;
        }
    }
}
