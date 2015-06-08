using UnityEngine;
using System.Collections;
using VTL.UI;

public class FireSpreadManagerLabelModeChanger : ToggleGroupListener
{
    public void Start()
    {
        FireSpreadManager mngr = Transform.FindObjectOfType<FireSpreadManager>();

        foreach (var toggle in toggles)
        {
            if (toggle.isOn)
                mngr.SetLabelMode(toggle.name);
        }

    }

    public override void ToggleStateChanged(string name, bool state)
    {
        // If the toggle was enabled notify the group
        if (state == true)
        {
            FireSpreadManager mngr = Transform.FindObjectOfType<FireSpreadManager>();
            result = name;
            mngr.SetLabelMode(name);
        }
    }
}
