using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private GameObject _heartSample;
        [SerializeField] private Sprite _heartEmpty;
        [SerializeField] private Sprite _heartHalf;
        [SerializeField] private Sprite _heartFull;

        private Image[] _hearts;
        private int _heartsCount = 10;
        private int _sectionsCount = 20;

        private void Awake()
        {
            _hearts = new Image[_heartsCount];

            for(int i = 0; i < _heartsCount; i++)
            {
                GameObject heart = Instantiate(_heartSample, transform);
                Image image = heart.GetComponent<Image>();
                image.sprite = _heartFull;
                _hearts[i] = image;
            }
        }

        public void SetHearts(int health, int maxHealth)
        {
            float healthPercent = (float)health / (float)maxHealth;
            int sectionsFilled = (int)Mathf.Ceil(healthPercent * _sectionsCount);
            int fullHearts = sectionsFilled / 2;
            bool hasHalfHeart = sectionsFilled % 2 == 1;

            for (int i = 0; i < _hearts.Length; i++)
            {
                if (i < fullHearts)
                {
                    _hearts[i].sprite = _heartFull;
                }

                if (i >= fullHearts)
                {
                    _hearts[i].sprite = _heartEmpty;
                }

                if (i == fullHearts && hasHalfHeart == true)
                {
                    _hearts[i].sprite = _heartHalf;
                }
            }
        }
    }
}