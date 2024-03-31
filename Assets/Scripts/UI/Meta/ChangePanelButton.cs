using UnityEngine;

namespace MetaUIElements
{
    public class ChangePanelButton : MonoBehaviour
    {
        [SerializeField] private MetaUI _metaUi;
        [SerializeField] private Panel _panel;

        public void OnClick()
        {
            ServiceLocator.Get<AudioService>().OnButtonClicked();
            _metaUi.SetActivePanel(_panel);
        }
    }
}