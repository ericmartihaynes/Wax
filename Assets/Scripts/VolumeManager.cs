using UnityEngine;

namespace DefaultNamespace
{
    public class VolumeManager : MonoBehaviour
    {
        public float volume { get; set; }



        public void updateVolume(float volume)
        {
            this.volume = volume;
        }

        public void volumenManager(AudioSource audio)
        {
            audio.volume = volume;
        }
        
        
    }
}