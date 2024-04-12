using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private Slider _soundVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _sensitivity;

        [field: SerializeField] public Button BackButton { get; private set; }

        private IReadOnlyGameDataService _gameDataService;

        public event Action<float> SoundVolumeChanged;
        public event Action<float> MusicVolumeChanged;
        public event Action<float> SensitivityChanged;

        private void Awake()
        {
            _gameDataService = ServiceLocator.Get<GameDataService>();

            _soundVolume.onValueChanged.AddListener(OnSoundVolumeChanged);
            _musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
            _sensitivity.onValueChanged.AddListener(OnSensitivityChanged);

            _soundVolume.value = _gameDataService.SoundVolume;
            _musicVolume.value = _gameDataService.MusicVolume;
            _sensitivity.value = _gameDataService.Sensitivity;
        }

        private void OnDestroy()
        {
            _soundVolume.onValueChanged.RemoveListener(OnSoundVolumeChanged);
            _musicVolume.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            _sensitivity.onValueChanged.RemoveListener(OnSensitivityChanged);
        }

        private void OnSoundVolumeChanged(float value)
        {
            SoundVolumeChanged?.Invoke(value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            MusicVolumeChanged?.Invoke(value);
        }

        private void OnSensitivityChanged(float value)
        {
            SensitivityChanged?.Invoke(value);
        }
    }
}