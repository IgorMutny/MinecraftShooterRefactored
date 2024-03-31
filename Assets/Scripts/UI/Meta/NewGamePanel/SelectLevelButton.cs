using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetaUIElements
{
    public class SelectLevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _selection;

        private NewGamePanel _newGamePanel;
        private LevelInfo _level;

        public void Initialize(LevelInfo level, NewGamePanel newGamePanel)
        {
            _level = level;
            _name.text = _level.Name;
            _image.sprite = _level.Image;
            _newGamePanel = newGamePanel;
        }

        public void SetOpen(bool value)
        {
            GetComponent<Button>().interactable = value;
            Color color = value ? Color.white : Color.gray;
            _name.color = color;
            _image.color = color;
        }

        public void SetSelected(bool value)
        {
            _selection.SetActive(value);
        }

        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            _newGamePanel.SelectLevel(_level);
        }
    }
}