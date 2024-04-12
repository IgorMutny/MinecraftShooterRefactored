using UnityEngine;

public class AdService : IService
{
    public AdService()
    {
        YG.YandexGame.CloseFullAdEvent += Resume;
        YG.YandexGame.ErrorFullAdEvent += Resume;
    }

    public void ShowFullScreenAd()
    {
        YG.YandexGame.FullscreenShow();
    }

    private void Resume()
    {
        IGameState currentState = ServiceLocator.Get<GameStateMachine>().CurrentState;

        if (currentState is CoreGameState)
        {
            ServiceLocator.Get<TimerWrapper>().AddSignal(0.1f, LockCursor);
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    ~AdService()
    {
        YG.YandexGame.CloseFullAdEvent -= Resume;
        YG.YandexGame.ErrorFullAdEvent -= Resume;
    }
}
