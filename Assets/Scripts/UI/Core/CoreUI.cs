using System;
using System.Collections;
using UnityEngine;

namespace CoreUIElements
{
    public class CoreUI : MonoBehaviour
    {
        [SerializeField] private MetaUIElements.BalanceWidget _balance;
        [SerializeField] private Panel _panel;
        [SerializeField] private Health _health;
        [SerializeField] private Armor _armor;
        [SerializeField] private Weapons _weapons;
        [SerializeField] private Rounds _rounds;
        [SerializeField] private Totem _totem;
        [SerializeField] private MessagePresenter _messagePresenter;
        [SerializeField] private YouDeadMessage _youDeadMessage;
        [SerializeField] private InGameMenu _inGameMenu;

        private Character _player;
        private LootCollection _lootCollection;

        public event Action ResumeButtonClicked;
        public event Action ExitButtonClicked;

        public void Initialize(Character player)
        {
            _lootCollection = ServiceLocator.Get<LootCollection>();
            _lootCollection.BalanceChanged += OnBalanceChanged;

            IReadOnlyGameDataService gameDataService = ServiceLocator.Get<GameDataService>();
            _balance.Initialize(gameDataService);

            _player = player;

            _player.Health.Cured += OnCured;
            _player.Health.Damaged += OnDamaged;
            _player.Health.Died += OnDied;

            _armor.SetArmors(_player.Health.Defence);

            _weapons.Create(_player.Inventory.GetAllWeapons());

            _player.Inventory.WeaponChanged += OnWeaponChanged;
            _player.Inventory.RoundsChanged += OnRoundsChanged;
            _player.Inventory.MaxRoundsChanged += OnMaxRoundsChanged;

            _totem.SetActive(_player.Health.CanBeResurrected);
            _player.Health.Resurrected += OnResurrected;

            _inGameMenu.gameObject.SetActive(false);
            _youDeadMessage.gameObject.SetActive(false);

            _inGameMenu.ResumeButton.onClick.AddListener(() => ResumeButtonClicked?.Invoke());
            _inGameMenu.ExitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());

        }

        private void OnDestroy()
        {
            _lootCollection.BalanceChanged -= OnBalanceChanged;

            _player.Health.Cured -= OnCured;
            _player.Health.Damaged -= OnDamaged;
            _player.Health.Died -= OnDied;
            _player.Health.Resurrected -= OnResurrected;

            _player.Inventory.WeaponChanged -= OnWeaponChanged;
            _player.Inventory.RoundsChanged -= OnRoundsChanged;
            _player.Inventory.MaxRoundsChanged -= OnMaxRoundsChanged;

            _inGameMenu.ResumeButton.onClick.RemoveAllListeners();
            _inGameMenu.ExitButton.onClick.RemoveAllListeners();
        }

        public void ShowMessage(string text, Color color)
        {
            _messagePresenter.ShowMessage(text, color);
        }

        public void SwitchInGameMenu(bool isPaused)
        {
            _inGameMenu.gameObject.SetActive(isPaused);
        }

        private void OnCured(int health)
        {
            _panel.Cure();
            _health.SetHearts(health, _player.Health.MaxHealth);
        }

        private void OnDamaged(int health, Character attacker)
        {
            _panel.Damage();
            _health.SetHearts(health, _player.Health.MaxHealth);
        }

        private void OnDied(Character attacker)
        {
            StartCoroutine(OnDiedCoroutine());
        }

        private void OnResurrected()
        {
            _totem.ShowResurrection(_player.Head);
            _totem.SetActive(_player.Health.CanBeResurrected);
        }

        private IEnumerator OnDiedCoroutine()
        {
            _panel.Die();
            _youDeadMessage.gameObject.SetActive(true);
            _youDeadMessage.Animate();
            yield return new WaitForSeconds(3f);
            Cursor.lockState = CursorLockMode.None;
            _inGameMenu.OnDied();
            _inGameMenu.gameObject.SetActive(true);
        }

        private void OnWeaponChanged(int index)
        {
            _weapons.ChangeWeapon(index);
        }

        private void OnMaxRoundsChanged(int amount)
        {
            _rounds.SetMaxRoundsAmount(amount);
        }

        private void OnRoundsChanged(int amount)
        {
            _rounds.SetRoundsAmount(amount);
        }

        private void OnBalanceChanged()
        {
            _balance.Reload();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Get<GameDataService>().Save();
        }
    }
}