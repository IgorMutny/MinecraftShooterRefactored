using UnityEngine;
using TMPro;

namespace MetaUIElements
{
    public class BalanceWidget : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _gold;
        [SerializeField] protected TextMeshProUGUI _diamonds;

        private IReadOnlyGameDataService _gameDataService;

        public void Initialize(IReadOnlyGameDataService gameDataService)
        {
            _gameDataService = gameDataService;
            SetValues();
        }

        public void Reload()
        {
            SetValues();
        }

        private void SetValues()
        {
            _gold.text = _gameDataService.Gold.ToString();
            _diamonds.text = _gameDataService.Diamonds.ToString();
        }
    }
}