using TMPro;
using UnityEngine;

namespace MetaUIElements
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;

        private ItemInfo _item;
        private ItemBrowser _browser;

        public void Initialize(ItemInfo item, ItemBrowser browser)
        {
            _browser = browser;
            _item = item;
            _name.text = _item.Name;
        }

        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            _browser.SetItem(_item);
        }
    }
}