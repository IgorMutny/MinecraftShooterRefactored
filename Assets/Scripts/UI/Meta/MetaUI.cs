using UnityEngine;

namespace MetaUIElements
{
    public class MetaUI : MonoBehaviour
    {
        [SerializeField] private Panel[] _panels;

        private MetaGame _metaGame;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;

            SetActivePanel(_panels[0]);
        }

        public void Initialize(MetaGame metaGame, ItemInfoCollection itemsCollection, IReadOnlyGameDataService gameDataService)
        {
            _metaGame = metaGame;

            foreach (Panel panel in _panels)
            {
                panel.Initialize(_metaGame, itemsCollection, gameDataService);
            }
        }

        public void SetActivePanel(Panel activePanel)
        {
            foreach (Panel panel in _panels)
            {
                panel.gameObject.SetActive(panel == activePanel);
            }
        }

        public void Reload()
        {
            foreach (Panel panel in _panels)
            {
                panel.Reload();
            }
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Get<GameDataService>().Save();
        }
    }
}