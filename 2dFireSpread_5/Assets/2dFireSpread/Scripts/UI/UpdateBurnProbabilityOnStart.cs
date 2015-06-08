using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateBurnProbabilityOnStart : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var value = GetComponent<Slider>().value;
        Transform.FindObjectOfType<FireSpreadManager>().SetBurnProbability(value);
    }
}
