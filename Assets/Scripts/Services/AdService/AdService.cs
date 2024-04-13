using UnityEngine;

public class AdService : IService
{
    public AdService()
    {
        YG.YandexGame.CloseFullAdEvent += LockCursor;
        YG.YandexGame.ErrorFullAdEvent += LockCursor;
    }

    public void ShowFullScreenAd()
    {
        YG.YandexGame.FullscreenShow();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    ~AdService()
    {
        YG.YandexGame.CloseFullAdEvent -= LockCursor;
        YG.YandexGame.ErrorFullAdEvent -= LockCursor;
    }
}
