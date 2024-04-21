using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CoreUIElements
{
    public class UIMobileInput : MonoBehaviour
    {
        [field: SerializeField] public Joystick MovementJoystick { get; private set; }
        [field: SerializeField] public Joystick RotationJoystick { get; private set; }
        [field: SerializeField] public UIMobileInputButton FireButton { get; private set; }
        [field: SerializeField] public UIMobileInputButton ReloadButton { get; private set; }
        [field: SerializeField] public Button PauseButton { get; private set; }

        public UIMobileInputButton[] CreateWeaponButtons(GameObject[] images)
        {
            UIMobileInputButton[] buttons = new UIMobileInputButton[images.Length];

            for(int i = 0; i < images.Length; i++)
            {
                buttons[i] = images[i].AddComponent<UIMobileInputButton>();
            }

            return buttons;
        }
    }
}