using UnityEngine;
using UnityEngine.UI;

namespace MetaUIElements
{
    public class OptionsPanel : Panel
    {
        [SerializeField] private Slider _soundVolume;
        [SerializeField] private Slider _musicVolume;

        private MetaGame _metaGame;
        private IReadOnlyGameDataService _gameDataService;

        public override void Initialize(
            MetaGame metaGame, 
            ItemInfoCollection itemsCollection, 
            IReadOnlyGameDataService gameDataService)
        {
            _metaGame = metaGame;
            _gameDataService = gameDataService;

            _soundVolume.onValueChanged.AddListener(OnSoundVolumeChanged);
            _musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);

            _soundVolume.value = _gameDataService.SoundVolume;
            _musicVolume.value = _gameDataService.MusicVolume;
        }

        public override void Reload() { }

        private void OnDestroy()
        {
            _soundVolume.onValueChanged.RemoveListener(OnSoundVolumeChanged);
            _musicVolume.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        }

        private void OnSoundVolumeChanged(float value)
        {
            _metaGame.ChangeSoundVolume(value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            _metaGame.ChangeMusicVolume(value);
        }
    }
}