using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }
    }
}