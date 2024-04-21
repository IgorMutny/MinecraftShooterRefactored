using CoreUIElements;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour
{
    private Character _player;
    private PauseHandler _pauseHandler;
    private IReadOnlyGameDataService _gameDataService;
    private CoreUI _ui;

    private Joystick _movementJoystick;
    private Joystick _rotationJoystick;
    private UIMobileInputButton _fireButton;
    private UIMobileInputButton _reloadButton;
    private UIMobileInputButton[] _weaponButtons;
    private Button _pauseButton;

    public void Initialize(Character player, CoreUI ui)
    {
        _player = player;
        _ui = ui;
        _pauseHandler = ServiceLocator.Get<PauseHandler>();
        _gameDataService = ServiceLocator.Get<GameDataService>();

        _movementJoystick = _ui.UIMobileInput.MovementJoystick;
        _rotationJoystick = _ui.UIMobileInput.RotationJoystick;
        _pauseButton = _ui.UIMobileInput.PauseButton;
        _fireButton = _ui.UIMobileInput.FireButton;
        _reloadButton = _ui.UIMobileInput.ReloadButton;
        _weaponButtons = _ui.GetWeaponButtons();

        _pauseButton.onClick.AddListener(Pause);
    }

    private void OnDestroy()
    {
        _pauseButton.onClick.RemoveListener(Pause);
    }

    private void Update()
    {
        if (_player != null && _player.IsAlive == true)
        {
            Vector3 movementInput = _movementJoystick.Direction;
            Vector3 rotationInput = _rotationJoystick.Direction * _gameDataService.Sensitivity;

            bool isAttacking = _fireButton.IsPressedProperty;
            bool isReloading = _reloadButton.IsPressedProperty;

            int numericWeaponIndex = -1;
            
            for(int i = 0; i < _weaponButtons.Length; i++)
            { 
                if (_weaponButtons[i].IsPressedProperty == true)
                {
                    numericWeaponIndex = i;
                }
            }

            _player.SetInput(movementInput, rotationInput,
                isAttacking, isReloading,
                numericWeaponIndex, 0);
        }
    }

    private void Pause()
    {
        _pauseHandler.Pause();
    }
}
