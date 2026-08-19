using System;
using UnityEngine;

namespace GameLogic.Audio
{
    [Serializable]
    public class SoundConfig
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }
}
