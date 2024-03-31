using UnityEngine;

namespace MetaUIElements
{
    public class PurchaseButton : MonoBehaviour
    {
        [SerializeField] private ItemBrowser _itemBrowser;

        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            _itemBrowser.OnPurchaseButtonClicked();
        }
    }
}