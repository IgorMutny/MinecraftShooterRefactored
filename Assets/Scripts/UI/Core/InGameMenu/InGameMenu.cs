using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class InGameMenu : MonoBehaviour
    {
        [field: SerializeField] public Button ResumeButton { get; private set; }
        [field: SerializeField] public Button ExitButton { get; private set; }

        public void OnDied()
        {
            ResumeButton.interactable = false;
        }
    }
}