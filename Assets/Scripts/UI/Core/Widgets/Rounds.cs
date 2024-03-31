using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class Rounds : MonoBehaviour
    {
        [SerializeField] private GameObject _roundSample;
        [SerializeField] private Sprite _roundFullImage;
        [SerializeField] private Sprite _roundEmptyImage;
        [SerializeField] private int _maxRoundImagesAmount;
        [SerializeField] private TextMeshProUGUI _additionalRoundsText;

        private int _maxRounds;
        private List<Image> _roundImages = new List<Image>();

        public void SetMaxRoundsAmount(int value)
        {
            int imagesCount = Mathf.Min(value, _maxRoundImagesAmount);

            if (imagesCount < _maxRounds)
            {
                for (int i = _roundImages.Count - 1; i >= imagesCount; i--)
                {
                    Destroy(_roundImages[i].gameObject);
                    _roundImages.RemoveAt(i);
                }
            }

            if (imagesCount > _maxRounds)
            {
                for (int i = _roundImages.Count; i < imagesCount; i++)
                {
                    Image hudRound = Instantiate(_roundSample, transform).GetComponent<Image>();
                    _roundImages.Add(hudRound);
                }
            }

            _maxRounds = value;
            SetRoundsText(value);
        }

        public void SetRoundsAmount(int value)
        {
            for (int i = 0; i < _roundImages.Count; ++i)
            {
                if (i < value)
                {
                    _roundImages[i].sprite = _roundFullImage;
                }
                else
                {
                    _roundImages[i].sprite = _roundEmptyImage;
                }
            }

            SetRoundsText(value);
        }

        private void SetRoundsText(int value)
        {
            if (value > _maxRoundImagesAmount)
            {
                string text = "+" + (value - _maxRoundImagesAmount);
                _additionalRoundsText.text = text;
            }
            else
            {
                _additionalRoundsText.text = string.Empty;
            }
        }
    }
}