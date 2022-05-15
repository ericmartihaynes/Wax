using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerShot, dropCasing, dropCoin, punch, hitmarker, deathSound, hurtSound;

    static AudioSource audiosrc;

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
        audiosrc = GetComponent<AudioSource>();
        audiosrc.volume = (float) 0.2;
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
                audiosrc.PlayOneShot(playerShot);
                break;
            case "hurt":
                audiosrc.PlayOneShot(hurtSound);
                break;
            case "dropCoin":
                audiosrc.PlayOneShot(dropCoin);
                break;
            case "dropCasing":
                audiosrc.PlayOneShot(dropCasing);
                break;
            case "hitmarker":
                audiosrc.PlayOneShot(hitmarker);
                break;
            case "death":
                audiosrc.PlayOneShot(deathSound);
                break;
            case "punch":
                audiosrc.PlayOneShot(punch);
                break;
        }
    }
}