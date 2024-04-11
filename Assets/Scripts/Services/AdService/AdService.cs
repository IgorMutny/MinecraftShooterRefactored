using UnityEngine;

public class AdService : IService
{
    private CursorLockMode _lockMode;

    public AdService()
    {
        YG.YandexGame.CloseFullAdEvent += Resume;
        YG.YandexGame.ErrorFullAdEvent += Resume;
    }

    public void ShowFullScreenAd()
    {
        _lockMode = Cursor.lockState;
        YG.YandexGame.FullscreenShow();
    }

    private void Resume()
    {
        Cursor.lockState = _lockMode;
    }

    ~AdService()
    {
        YG.YandexGame.CloseFullAdEvent -= Resume;
        YG.YandexGame.ErrorFullAdEvent -= Resume;
    }
}
