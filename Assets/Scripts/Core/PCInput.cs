using UnityEngine;

public class PCInput : MonoBehaviour
{
    private float _mouseSensitivity = 2000;

    private readonly KeyCode _forwardKey = KeyCode.W;
    private readonly KeyCode _backwardKey = KeyCode.S;
    private readonly KeyCode _leftKey = KeyCode.A;
    private readonly KeyCode _rightKey = KeyCode.D;
    private readonly KeyCode _attackKey = KeyCode.None;
    private readonly KeyCode _reloadKey = KeyCode.None;
    private readonly KeyCode _nextWeaponKey = KeyCode.None;
    private readonly KeyCode _prevWeaponKey = KeyCode.None;

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

            if (Input.GetKeyDown(KeyCode.E))
            {
                _player.GetCure(Random.Range(0, 250));
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _player.GetDamage(Random.Range(0, 250), DamageType.Physical, null);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _pauseHandler.TryPause();
            }
        }
    }

    private Vector3 GetMovement()
    {
        float vertical = 0;

        if (Input.GetKey(_forwardKey) == true)
        {
            vertical += 1;
        }

        if (Input.GetKey(_backwardKey) == true)
        {
            vertical -= 1;
        }

        float horizontal = 0;

        if (Input.GetKey(_rightKey) == true)
        {
            horizontal += 1;
        }

        if (Input.GetKey(_leftKey) == true)
        {
            horizontal -= 1;
        }

        Vector3 movementInput = new Vector3(horizontal, vertical, 0).normalized;
        return movementInput;
    }

    private Vector3 GetRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        Vector3 rotationInput = new Vector3(mouseX, mouseY, 0);
        return rotationInput;
    }

    private bool GetAttack()
    {
        return Input.GetMouseButton(0) || Input.GetKey(_attackKey);
    }

    private bool GetReload()
    {
        return Input.GetMouseButton(1) || Input.GetKey(_reloadKey);
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
        if (Input.GetKey(_prevWeaponKey) == true || Input.mouseScrollDelta.y < 0)
        {
            return -1;
        }

        if (Input.GetKey(_nextWeaponKey) == true || Input.mouseScrollDelta.y > 0)
        {
            return 1;
        }

        return 0;
    }
}
