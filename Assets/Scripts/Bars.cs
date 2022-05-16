using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    public void SetValue(float value) {
        slider.value = value;
    }
}
