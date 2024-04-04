using UnityEngine;

namespace CoreUIElements
{
    public class ExitButton : MonoBehaviour
    {
        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
        }
    }
}