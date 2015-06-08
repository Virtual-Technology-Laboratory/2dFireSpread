using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace VTL.UI
{
    public class SliderTextUpdate : MonoBehaviour
    {
        public string fmtString = "P";
        Text text;

        void Start()
        {
            text = transform.FindChild("Handle Slide Area")
                            .FindChild("Handle")
                            .FindChild("Text")
                            .GetComponent<Text>();

            Slider slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener((value) => OnValueChange(value));
        }

        public void OnValueChange(float value)
        {
            text.text = value.ToString(fmtString);
        }
    }
}