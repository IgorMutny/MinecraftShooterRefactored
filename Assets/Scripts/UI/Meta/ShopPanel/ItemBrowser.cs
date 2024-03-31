using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MetaUIElements
{
    public class ItemBrowser : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private ItemModelView _modelView;
        [SerializeField] private TextMeshProUGUI _priceGold;
        [SerializeField] private TextMeshProUGUI _priceDiamonds;
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private GameObject _purchasedIcon;

        private MetaGame _metaGame;
        private IReadOnlyGameDataService _gameDataService;
        private ItemInfo _item;

        public void Initialize(MetaGame metaGame, IReadOnlyGameDataService gameDataService)
        {
            _metaGame = metaGame;
            _gameDataService = gameDataService;
            _purchaseButton.interactable = false;
            _purchasedIcon.SetActive(false);
        }

        public void Reload()
        {
            SetInfo();
        }

        public void SetItem(ItemInfo item)
        {
            _item = item;

            SetInfo();
        }

        private void SetInfo()
        {
            if (_item != null)
            {
                _name.text = _item.Name;
                _description.text = _item.Description;
                _modelView.SetModel(_item.Model);
                _priceGold.text = _item.PriceInGold.ToString();
                _priceDiamonds.text = _item.PriceInDiamonds.ToString();
                SetPurchaseButtonAvailability(_item);
                SetPurchasedIconVisibility(_item);
            }
        }

        private void SetPurchaseButtonAvailability(ItemInfo item)
        {
            bool hasNotPurchased = _gameDataService.HasItem(item.Id) == false;
            bool hasEnoughGold = _gameDataService.Gold >= item.PriceInGold;
            bool hasEnoughDiamonds = _gameDataService.Diamonds >= item.PriceInDiamonds;

            bool isInteractable = hasNotPurchased && hasEnoughDiamonds && hasEnoughGold;
            _purchaseButton.interactable = isInteractable;
        }

        private void SetPurchasedIconVisibility(ItemInfo item)
        {
            _purchasedIcon.SetActive(_gameDataService.HasItem(item.Id));
        }

        public void OnPurchaseButtonClicked()
        {
            _metaGame.TryBuyItem(_item);
        }
    }
}