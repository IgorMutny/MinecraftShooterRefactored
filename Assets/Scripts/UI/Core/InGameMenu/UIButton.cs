using UnityEngine;

namespace CoreUIElements
{
    public class UIButton : MonoBehaviour
    {
        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
        }
    }
}