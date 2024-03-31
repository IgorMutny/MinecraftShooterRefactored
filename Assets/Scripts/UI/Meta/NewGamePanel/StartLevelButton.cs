using UnityEngine;

namespace MetaUIElements
{
    public class StartLevelButton : MonoBehaviour
    {
        private NewGamePanel _newGamePanel;

        public void Initialize(NewGamePanel newGamePanel)
        {
            _newGamePanel = newGamePanel;
        }

        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            _newGamePanel.StartLevel();
        }
    }
}