using UnityEngine;

namespace MetaUIElements
{
    public class ShopPanel : Panel
    {
        [SerializeField] private BalanceWidget _balanceWidget;
        [SerializeField] private ItemsList _itemsList;
        [SerializeField] private ItemBrowser _itemBrowser;

        private MetaGame _metaGame;
        private ItemInfoCollection _itemsCollection;
        private IReadOnlyGameDataService _gameDataService;

        public override void Initialize(
            MetaGame metaGame, 
            ItemInfoCollection itemsCollection, 
            IReadOnlyGameDataService gameDataService)
        {
            _metaGame = metaGame;
            _itemsCollection = itemsCollection;
            _gameDataService = gameDataService;

            _balanceWidget.Initialize(_gameDataService);
            _itemsList.Initialize(_itemsCollection);
            _itemBrowser.Initialize(_metaGame, _gameDataService);
        }

        public override void Reload()
        {
            _balanceWidget.Reload();
            _itemBrowser.Reload();
        }
    }
}