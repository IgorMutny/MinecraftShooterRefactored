using UnityEngine;

namespace CoreUIElements
{
    public class ResumeButton : MonoBehaviour
    {
        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            PauseHandler pauseHandler = ServiceLocator.Get<PauseHandler>();
            pauseHandler.Resume();
        }
    }
}