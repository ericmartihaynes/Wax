using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MixerController : MonoBehaviour
{
    public AudioMixer audmix;
    // Start is called before the first frame update
    public void SetVolume(float sliderValue) {
        audmix.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }
}
