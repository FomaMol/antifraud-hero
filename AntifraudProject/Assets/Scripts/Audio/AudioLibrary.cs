using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace GameLogic.Audio
{
    public class AudioLibrary : MonoBehaviour
    {
        [Inject] private readonly AudioService _audioService;

        [SerializeField] private List<SoundConfig> _sounds;
        public IReadOnlyList<SoundConfig> Sounds => _sounds;

        private void OnDestroy()
        {
            if (_audioService != null)
                _audioService.UnregisterLibrary(this);
        }
    }
}
