using Udar.DesignPatterns.Singleton;
using UnityEngine;

namespace Udarverse
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _sfx;


        public void Play(AudioClip clip,float pitchLevel)
        {
            _sfx.clip = clip;
            _sfx.pitch = pitchLevel;
            _sfx.Play();
        }

        public void PlayOneShot(AudioClip clip)
        {
            _sfx.PlayOneShot(clip);
        }


       

    }

}
