using UnityEngine;

namespace MetaUIElements
{
    public class ItemsList : MonoBehaviour
    {
        [SerializeField] private string _weapons;
        [SerializeField] private string _armors;
        [SerializeField] private string _totems;
        [SerializeField] private GameObject _itemButtonSample;
        [SerializeField] private GameObject _itemSeparatorSample;
        [SerializeField] private Transform _content;
        [SerializeField] private ItemBrowser _browser;

        private float _height;

        public void Initialize(ItemInfoCollection items)
        {
            _height = 0;

            CreateItemGroup(items.Weapons, _weapons);
            CreateItemGroup(items.Armors, _armors);
            CreateItemGroup(items.Totems, _totems);

            ResizeContent(_height);
        }

        private void CreateItemGroup(ItemInfo[] items, string name)
        {
            AddSeparator(name);

            if (items.Length > 0)
            {
                foreach (ItemInfo item in items)
                {
                    AddButton(item);
                }
            }
        }

        private void AddSeparator(string name)
        {
            GameObject separator = Instantiate(_itemSeparatorSample, _content);
            separator.GetComponent<ItemSeparator>().SetText(name);
            _height += separator.GetComponent<RectTransform>().sizeDelta.y;
        }

        private void AddButton(ItemInfo item)
        {
            GameObject button = Instantiate(_itemButtonSample, _content);
            button.GetComponent<ItemButton>().Initialize(item, _browser);
            _height += button.GetComponent<RectTransform>().sizeDelta.y;
        }

        private void ResizeContent(float height)
        {
            RectTransform contentRect = _content.GetComponent<RectTransform>();
            Vector2 sizeDelta = contentRect.sizeDelta;
            sizeDelta.y = height;
            contentRect.sizeDelta = sizeDelta;
        }
    }
}