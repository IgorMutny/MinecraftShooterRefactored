using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private Slider _soundVolume;
        [SerializeField] private Slider _musicVolume;

        [field: SerializeField] public UnityEngine.UI.Button BackButton { get; private set; }

        private IReadOnlyGameDataService _gameDataService;

        public event Action<float> SoundVolumeChanged;
        public event Action<float> MusicVolumeChanged;

        private void Awake()
        {
            _gameDataService = ServiceLocator.Get<GameDataService>();

            _soundVolume.onValueChanged.AddListener(OnSoundVolumeChanged);
            _musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);

            _soundVolume.value = _gameDataService.SoundVolume;
            _musicVolume.value = _gameDataService.MusicVolume;
        }

        private void OnDestroy()
        {
            _soundVolume.onValueChanged.RemoveListener(OnSoundVolumeChanged);
            _musicVolume.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        }

        private void OnSoundVolumeChanged(float value)
        {
            SoundVolumeChanged?.Invoke(value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            MusicVolumeChanged?.Invoke(value);
        }
    }
}