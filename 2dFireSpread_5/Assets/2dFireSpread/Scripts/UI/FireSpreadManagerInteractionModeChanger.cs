using UnityEngine;
using System.Collections;

public class FireSpreadManagerInteractionModeChanger : ToggleGroupListener
{
    public override void ToggleStateChanged(string name, bool state)
    {
        // If the toggle was enabled notify the group
        if (state == true)
        {
            FireSpreadManager mngr = Transform.FindObjectOfType<FireSpreadManager>();
            result = name;
            mngr.SetInteractionMode(name);
        }
    }
}
