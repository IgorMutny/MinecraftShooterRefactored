using UnityEngine;
using MetaUIElements;
using YG;

public class AdService : IService
{
    public AdService()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    public void ShowFullScreenAd()
    {
        YandexGame.FullscreenShow();
    }

    public void ShowRewarded()
    {
        YandexGame.RewVideoShow(0);
    }

    ~AdService()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    private void Rewarded(int i)
    {
        ServiceLocator.Get<GameDataService>().AddDiamonds(1);
        MetaUI _ui = GameObject.FindObjectOfType<MetaUI>();
        if (_ui != null)
        {
            _ui.Reload();
        }
    }
}
