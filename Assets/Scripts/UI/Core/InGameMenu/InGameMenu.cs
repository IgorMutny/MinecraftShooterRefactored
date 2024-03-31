using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;

        public void OnDied()
        {
            _continueButton.interactable = false;
        }
    }
}