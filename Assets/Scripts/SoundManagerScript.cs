using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    private static AudioClip playerShot, dropCasing, dropCoin, punch, hitmarker, deathSound, hurtSound, jump, walk, metalPower;
    static AudioSource audio;

    private VolumeManager vol;
    // Start is called before the first frame update
    void Start()
    {
        playerShot = Resources.Load<AudioClip>("gunshot5");
        dropCasing = Resources.Load<AudioClip>("drop-casing");
        dropCoin = Resources.Load<AudioClip>("drop-coin");
        punch = Resources.Load<AudioClip>("punch");
        hitmarker = Resources.Load<AudioClip>("hitmarker");
        deathSound = Resources.Load<AudioClip>("death-sound");
        hurtSound = Resources.Load<AudioClip>("hurtSound");
        metalPower = Resources.Load<AudioClip>("metal-power");
        walk = Resources.Load<AudioClip>("walk");
        jump = Resources.Load<AudioClip>("jump");

        audio = GetComponent<AudioSource>();
        vol.volumenManager(audio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void playSound(string clip)
    {
        switch (clip)
        {
            case "fire":
                audio.PlayOneShot(playerShot);
                break;
            case "hurt":
                audio.PlayOneShot(hurtSound);
                break;
            case "dropCoin":
                audio.PlayOneShot(dropCoin);
                break;
            case "dropCasing":
                audio.PlayOneShot(dropCasing);
                break;
            case "hitmarker":
                audio.PlayOneShot(hitmarker);
                break;
            case "death":
                audio.PlayOneShot(deathSound);
                break;
            case "punch":
                audio.PlayOneShot(punch);
                break;
            case "jump":
                audio.PlayOneShot(jump);
                break;
        }
    }
}
