using UnityEngine;

namespace CoreUIElements
{
    public class Weapons : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _weaponSample;
        [SerializeField] private GameObject _selection;

        private GameObject[] _weaponImages;

        private void Awake()
        {
            int amount = ServiceLocator.Get<SettingsService>()
                .Get<ItemInfoCollection>().Weapons.Length;
            _weaponImages = new GameObject[amount];
        }

        public void Create(WeaponInfo[] weaponInfos)
        { 
            foreach (var weaponInfo in weaponInfos)
            {
                if (weaponInfo != null)
                { 
                    AddWeapon(weaponInfo);
                }
            }
        }

        private void AddWeapon(WeaponInfo weaponInfo)
        {
            for (int i = 0; i < _weaponImages.Length; i++)
            {
                if (_weaponImages[i] == null)
                {
                    GameObject weapon = Instantiate(_weaponSample, _container);
                    Sprite icon = weaponInfo.Icon;
                    weapon.GetComponent<Weapon>().SetIcon(icon);

                    _weaponImages[i] = weapon;

                    return;
                }
            }
        }

        public void ChangeWeapon(int index)
        {
            _selection.transform.position = _weaponImages[index].transform.position;
        }

        public GameObject[] GetWeaponImages()
        {
            return _weaponImages;
        }
    }
}