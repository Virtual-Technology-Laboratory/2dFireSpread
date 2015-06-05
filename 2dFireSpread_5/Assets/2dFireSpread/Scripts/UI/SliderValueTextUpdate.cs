using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderValueTextUpdate : MonoBehaviour
{

    Text text;

    // Use this for initialization
    void Start()
    {
        text = transform.FindChild("Handle Slide Area")
                        .FindChild("Handle")
                        .FindChild("Text")
                        .GetComponent<Text>();

        GetComponent<Slider>().onValueChanged.AddListener((value) => OnValueChange(value));
    }

    // Update is called once per frame
    public void OnValueChange(float value)
    {
        text.text = value.ToString("P1");
    }
}
