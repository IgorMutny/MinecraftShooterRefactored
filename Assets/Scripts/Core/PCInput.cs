using UnityEngine;

public class PCInput : MonoBehaviour
{
    private float _mouseSensitivity = 1000;

    private readonly string _vertical = "Vertical";
    private readonly string _horizontal = "Horizontal";
    private readonly string _mouseX = "Mouse X";
    private readonly string _mouseY = "Mouse Y";
    private readonly string _fire = "Fire1";
    private readonly string _reload = "Fire2";

    private readonly KeyCode[] _changeWeaponKeys = new KeyCode[]
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0,
    };

    private Character _player;
    private PauseHandler _pauseHandler;

    public void Initialize(Character player)
    {
        _player = player;
        _pauseHandler = ServiceLocator.Get<PauseHandler>();
    }

    private void Update()
    {
        if (_player != null && _player.IsAlive == true)
        {
            Vector3 movementInput = GetMovement();
            Vector3 rotationInput = GetRotation();

            bool isAttacking = GetAttack();
            bool isReloading = GetReload();
            int numericWeaponIndex = GetChangeNumericWeapon();
            int prevNextWeaponIndex = GetChangePrevNextWeapon();

            _player.SetInput(movementInput, rotationInput,
                isAttacking, isReloading,
                numericWeaponIndex, prevNextWeaponIndex);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _pauseHandler.Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ServiceLocator.Get<TimerWrapper>().GetSignalsAmount();
        }
    }

    private Vector3 GetMovement()
    {
        float vertical = Input.GetAxis(_vertical);
        float horizontal = Input.GetAxis(_horizontal);

        Vector3 movementInput = new Vector3(horizontal, vertical, 0).normalized;
        return movementInput;
    }

    private Vector3 GetRotation()
    {
        float mouseX = Input.GetAxis(_mouseX) * _mouseSensitivity;
        float mouseY = Input.GetAxis(_mouseY) * _mouseSensitivity;
        Vector3 rotationInput = new Vector3(mouseX, mouseY, 0);
        return rotationInput;
    }

    private bool GetAttack()
    {
        return Input.GetAxis(_fire) > 0;
    }

    private bool GetReload()
    {
        return Input.GetAxis(_reload) > 0;
    }

    private int GetChangeNumericWeapon()
    {
        for (int i = 0; i < _changeWeaponKeys.Length; i++)
        {
            if (Input.GetKeyDown(_changeWeaponKeys[i]) == true)
            {
                return i;
            }
        }

        return -1;
    }

    private int GetChangePrevNextWeapon()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            return -1;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            return 1;
        }

        return 0;
    }
}
