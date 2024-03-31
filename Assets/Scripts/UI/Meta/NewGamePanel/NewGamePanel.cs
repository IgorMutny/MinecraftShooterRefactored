using UnityEngine;

namespace MetaUIElements
{
    public class NewGamePanel : Panel
    {
        [SerializeField] private GameObject _selectLevelButtonSample;
        [SerializeField] private StartLevelButton _startLevelButton;
        [SerializeField] private RectTransform _levelsList;

        private LevelInfo[] _levels;
        private SelectLevelButton[] _selectLevelButtons;
        private MetaGame _metaGame;
        private IReadOnlyGameDataService _gameDataService;

        public override void Initialize(
            MetaGame metaGame,
            ItemInfoCollection itemsCollection,
            IReadOnlyGameDataService gameDataService)
        {
            _metaGame = metaGame;
            _gameDataService = gameDataService;
            _levels =
                ServiceLocator.Get<SettingsService>().Get<LevelInfoCollection>().Levels;

            CreateLevelSelectionButtons();

            _startLevelButton.Initialize(this);
        }

        public override void Reload()
        {
            for (int i = 0; i < _levels.Length; i++)
            {
                SetOpenAndSelected(_levels[i], _selectLevelButtons[i]);
            }
        }

        public void SelectLevel(LevelInfo level)
        {
            _metaGame.TrySelectLevel(level);
        }

        private void CreateLevelSelectionButtons()
        {
            _selectLevelButtons = new SelectLevelButton[_levels.Length];

            for (int i = 0; i < _levels.Length; i++)
            {
                _selectLevelButtons[i] =
                    Instantiate(_selectLevelButtonSample, _levelsList)
                    .GetComponent<SelectLevelButton>();

                _selectLevelButtons[i].Initialize(_levels[i], this);

                SetOpenAndSelected(_levels[i], _selectLevelButtons[i]);
            }
        }

        private void SetOpenAndSelected(LevelInfo level, SelectLevelButton selectLevelButton)
        {
            bool isOpen = _gameDataService.IsLevelOpen(level.Id);
            selectLevelButton.SetOpen(isOpen);

            bool isSelected = _gameDataService.SelectedLevel == level.Id;
            selectLevelButton.SetSelected(isSelected);
        }

        public void StartLevel()
        {
            _metaGame.StartLevel();
        }
    }
}