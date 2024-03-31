using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _cureColor;
        [SerializeField] private Color _damageColor;
        [SerializeField] private float _blinkTime;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Damage()
        {
            DOTween.Sequence()
                .Append(_image.DOColor(_damageColor, _blinkTime))
                .Append(_image.DOColor(_normalColor, _blinkTime));
        }

        public void Cure()
        {
            DOTween.Sequence()
                .Append(_image.DOColor(_cureColor, _blinkTime))
                .Append(_image.DOColor(_normalColor, _blinkTime));
        }

        public void Die()
        {
            DOTween.Sequence()
                .Append(_image.DOColor(_damageColor, 1));
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }
    }
}