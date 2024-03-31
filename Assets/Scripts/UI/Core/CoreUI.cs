using System.Collections;
using UnityEngine;

namespace CoreUIElements
{
    public class CoreUI : MonoBehaviour
    {
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
        private PauseHandler _pauseHandler;

        public void Initialize(Character player)
        {
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

            _pauseHandler = ServiceLocator.Get<PauseHandler>();
            _pauseHandler.PauseSwitched += OnPauseSwitched;
            _inGameMenu.gameObject.SetActive(false);
            _youDeadMessage.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _player.Health.Cured -= OnCured;
            _player.Health.Damaged -= OnDamaged;
            _player.Health.Died -= OnDied;
            _player.Health.Resurrected -= OnResurrected;

            _player.Inventory.WeaponChanged -= OnWeaponChanged;
            _player.Inventory.RoundsChanged -= OnRoundsChanged;
            _player.Inventory.MaxRoundsChanged -= OnMaxRoundsChanged;

            _pauseHandler.PauseSwitched -= OnPauseSwitched;
        }

        public void ShowMessage(string text, Color color)
        {
            _messagePresenter.ShowMessage(text, color);
        }

        private void OnCured(int health)
        {
            _panel.Cure();
            _health.SetHearts(health, _player.Health.MaxHealth);
        }

        private void OnDamaged(int health)
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

        private void OnPauseSwitched(bool isPaused)
        {
            _inGameMenu.gameObject.SetActive(isPaused);
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Get<GameDataService>().Save();
        }
    }
}