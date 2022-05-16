using Unity.QuickSearch;
using UnityEngine;

namespace DefaultNamespace
{
    public class VolumeManager
    {
        private AudioSource audio;
        public float volume = 1f;
        
        VolumeManager(AudioSource audio, float volume)
        {
            this.audio = audio;
            this.volume = volume;
            
        }

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