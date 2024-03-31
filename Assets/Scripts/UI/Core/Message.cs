using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CoreUIElements
{
    public class Message : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            Destroy(gameObject, 3f);
        }

        public void Initialize(string text, Color color)
        {
            _text.text = text;
            _text.color = color;

            Animate();
        }

        private void Animate()
        {
            RectTransform rect = GetComponent<RectTransform>();

            DOTween.Sequence()
                .Append(rect.DOScale(0f, 0f))
                .Append(rect.DOScale(1f, 0.75f))
                .AppendInterval(1.5f)
                .Append(rect.DOScale(0f, 0.75f))
                .Insert(0f, rect.DORotate(new Vector3(0, 0, 15), 0.25f))
                .Insert(0.25f, rect.DORotate(new Vector3(0, 0, 0), 0.25f))
                .Insert(0.5f, rect.DORotate(new Vector3(0, 0, 15), 0.25f))
                .Insert(2.25f, rect.DORotate(new Vector3(0, 0, 0), 0.25f))
                .Insert(2.5f, rect.DORotate(new Vector3(0, 0, 15), 0.25f))
                .Insert(2.75f, rect.DORotate(new Vector3(0, 0, 0), 0.25f))
                .Insert(0f, rect.DOAnchorMin(new Vector2(0, 0.5f), 3f));
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }
    }
}