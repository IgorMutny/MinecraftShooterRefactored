using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetaUIElements
{
    public class RewardedButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;
        [SerializeField] private Color _color1;
        [SerializeField] private Color _color2;
        [SerializeField] private float _blinkDelay;

        private bool _isColored2;
        private float _counter;

        private void Awake()
        {
            _isColored2 = false;
            _counter = _blinkDelay;
        }

        private void FixedUpdate()
        {
            _counter -= Time.fixedDeltaTime;

            if (_counter <= 0)
            {
                ChangeColor();
                _counter += _blinkDelay;
            }
        }

        private void ChangeColor()
        {
            if (_isColored2 == false)
            {
                _text.color = _color2;
                _image.color = _color2;
                _isColored2 = true;
            }
            else
            {
                _text.color = _color1;
                _image.color = Color.white;
                _isColored2 = false;
            }
        }

        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            ServiceLocator.Get<AdService>().ShowRewarded();
        }
    }
}