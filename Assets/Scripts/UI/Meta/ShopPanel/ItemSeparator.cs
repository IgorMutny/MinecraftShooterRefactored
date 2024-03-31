using TMPro;
using UnityEngine;

namespace MetaUIElements
{
    public class ItemSeparator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}