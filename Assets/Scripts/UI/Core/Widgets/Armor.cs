using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class Armor : MonoBehaviour
    {
        [SerializeField] private GameObject _armorSample;
        [SerializeField] private Sprite _armorEmpty;
        [SerializeField] private Sprite _armorHalf;
        [SerializeField] private Sprite _armorFull;

        private Image[] _armors;
        private int _armorsCount = 10;
        private int _sectionsCount = 20;

        private void Awake()
        {
            _armors = new Image[_armorsCount];

            for(int i = 0; i < _armorsCount; i++)
            {
                GameObject armor = Instantiate(_armorSample, transform);
                Image image = armor.GetComponent<Image>();
                image.sprite = _armorFull;
                _armors[i] = image;
            }
        }

        public void SetArmors(float defence)
        {
            int sectionsFilled = (int)Mathf.Ceil(defence * _sectionsCount);
            int fullArmors = sectionsFilled / 2;
            bool hasHalfArmor = sectionsFilled % 2 == 1;

            for (int i = 0; i < _armors.Length; i++)
            {
                if (i < fullArmors)
                {
                    _armors[i].sprite = _armorFull;
                }

                if (i >= fullArmors)
                {
                    _armors[i].sprite = _armorEmpty;
                }

                if (i == fullArmors && hasHalfArmor == true)
                {
                    _armors[i].sprite = _armorHalf;
                }
            }
        }
    }
}