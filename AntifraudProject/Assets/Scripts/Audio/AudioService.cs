using System.Collections.Generic;
using common.Utility;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameLogic.Audio
{
    public class AudioService : MonoBehaviour, IInitializable
    {
        [Inject] private readonly PersistingService _persistingService;

        private const int POOL_SIZE = 16;

        [SerializeField] private AudioLibrary _audioLibrary;

        private Queue<AudioSource> _free;
        private OrderedDictionary<AudioSource, SoundConfig> _active;
        private Dictionary<string, SoundConfig> _soundMap;
        private readonly List<AudioLibrary> _libraries = new();

        public void Initialize()
        {
            InitPool();
            _soundMap = new Dictionary<string, SoundConfig>();
            ApplySoundsEnabled(_persistingService.SoundsEnabled);
            _persistingService.OnSoundsEnabledUpdate += ApplySoundsEnabled;
            RegisterLibrary(_audioLibrary);
            
            Play("background", looped: true);
        }

        private void OnDestroy()
        {
            if (_persistingService != null)
                _persistingService.OnSoundsEnabledUpdate -= ApplySoundsEnabled;
        }

        private void ApplySoundsEnabled(bool enabled)
        {
            AudioListener.volume = enabled ? 1f : 0f;
        }

        private void InitPool()
        {
            _free = new Queue<AudioSource>(POOL_SIZE);
            _active = new OrderedDictionary<AudioSource, SoundConfig>(POOL_SIZE);

            for (int i = 0; i < POOL_SIZE; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f;
                _free.Enqueue(source);
            }
        }

        private void RebuildSoundMap()
        {
            _soundMap.Clear();
            foreach (var library in _libraries)
            {
                foreach (var config in library.Sounds)
                {
                    if (string.IsNullOrEmpty(config.id)) continue;
                    _soundMap[config.id] = config;
                }
            }
        }

        public void Play(string id, bool looped = false)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (!_soundMap.TryGetValue(id, out var config))
            {
                Debug.LogWarning($"[AudioService] Sound not found: {id}");
                return;
            }

            var source = GetFreeSource();
            ApplyConfig(source, config, looped);
            source.Play();
        }

        public void Stop(string id)
        {
            if (!_soundMap.TryGetValue(id, out var config)) return;

            var toRelease = new List<AudioSource>();
            foreach (var pair in _active)
            {
                if (pair.Key.isPlaying && pair.Key.clip == config.clip)
                    toRelease.Add(pair.Key);
            }

            foreach (var source in toRelease)
                ReleaseSource(source);
        }

        public void StopAll()
        {
            var toRelease = new List<AudioSource>(_active.Count);
            foreach (var pair in _active)
                toRelease.Add(pair.Key);

            foreach (var source in toRelease)
                ReleaseSource(source);
        }

        private AudioSource GetFreeSource()
        {
            RecycleStopped();

            if (_free.Count > 0)
                return _free.Dequeue();

            foreach (var pair in _active)
            {
                if (!pair.Key.loop)
                {
                    var source = pair.Key;
                    ReleaseSource(source);
                    return source;
                }
            }

            var oldest = _active.GetKeyAt(0);
            ReleaseSource(oldest);
            return oldest;
        }

        private void RecycleStopped()
        {
            var stopped = new List<AudioSource>();
            foreach (var pair in _active)
            {
                if (!pair.Key.isPlaying && !pair.Key.loop)
                    stopped.Add(pair.Key);
            }

            foreach (var source in stopped)
                ReleaseSource(source);
        }

        private void ReleaseSource(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            _active.Remove(source);
            _free.Enqueue(source);
        }

        private void ApplyConfig(AudioSource source, SoundConfig config, bool looped)
        {
            source.clip = config.clip;
            source.volume = config.volume;
            source.loop = looped;
            source.spatialBlend = 0f;
            _active[source] = config;
        }

        public void RegisterLibrary(AudioLibrary audioLibrary)
        {
            if (audioLibrary == null) return;
            if (_libraries.Contains(audioLibrary)) return;
            _libraries.Add(audioLibrary);
            RebuildSoundMap();
        }

        public void UnregisterLibrary(AudioLibrary audioLibrary)
        {
            if (!_libraries.Remove(audioLibrary)) return;
            StopLibrarySounds(audioLibrary);
            RebuildSoundMap();
        }

        private void StopLibrarySounds(AudioLibrary audioLibrary)
        {
            var librarySounds = new HashSet<SoundConfig>(audioLibrary.Sounds);
            var toRelease = new List<AudioSource>();
            foreach (var pair in _active)
            {
                if (librarySounds.Contains(pair.Value))
                    toRelease.Add(pair.Key);
            }

            foreach (var source in toRelease)
                ReleaseSource(source);
        }
    }
}
