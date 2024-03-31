using UnityEngine;

namespace MetaUIElements
{
    public abstract class Panel : MonoBehaviour
    {
        public abstract void Initialize(
            MetaGame metaGame, 
            ItemInfoCollection itemsCollection, 
            IReadOnlyGameDataService gameDataService);

        public abstract void Reload();
    }
}