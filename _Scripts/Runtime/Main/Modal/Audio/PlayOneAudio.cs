using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udarverse.Audio
{
    public class PlayOneAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [Range(-3, 3)]
        [SerializeField] private float _pitchLevel = 1f;
        public void Play(float addition = 0)
        {
            var add = (1 - addition) * 0.8f * _pitchLevel;
            AudioManager.Instance.Play(_clip, _pitchLevel + add);
        }
    }
}

