using UnityEngine;

namespace CoreUIElements
{
    public class ExitButton : MonoBehaviour
    {
        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            GameStateMachine gameStateMachine = ServiceLocator.Get<GameStateMachine>();
            gameStateMachine.SetState(new MetaGameState());
        }
    }
}