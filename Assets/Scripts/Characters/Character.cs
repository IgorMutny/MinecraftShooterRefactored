using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public Transform Head { get; private set; }
    [field: SerializeField] public Transform UpperObstacleChecker { get; private set; }
    [field: SerializeField] public Transform LowerObstacleChecker { get; private set; }
    [field: SerializeField] public Transform GroundChecker { get; private set; }

    private bool _isInitialized = false;
    private bool _isAlive;

    private CharacterView _view;
    private CharacterInfo _characterInfo;
    private Movement _movement;
    private Inventory _inventory;
    private Health _health;

    private float _damageMultiplier = 1;
    private float _speedMultiplier = 1;

    public bool IsAlive => _isAlive;
    public float SpeedMultiplier => _speedMultiplier;
    public float DamageMultiplier => _damageMultiplier;

    public IReadOnlyInventory Inventory => _inventory;
    public IReadOnlyHealth Health => _health;

    private void Awake()
    {
        _view = GetComponent<CharacterView>();
    }

    public void Initialize(CharacterInfo characterInfo, bool isPlayer)
    {
        _characterInfo = characterInfo;
        _isAlive = true;

        AddMovement();
        AddInventory(isPlayer);
        AddHealth(isPlayer);

        _isInitialized = true;
    }

    #region InitializationMethods
    private void AddMovement()
    {
        switch (_characterInfo.MovementType)
        {
            case MovementType.Walking: _movement = new WalkingMovement(this, _characterInfo); break;
        }
    }

    private void AddInventory(bool isPlayer)
    {
        if (isPlayer == true)
        {
            IReadOnlyGameDataService gameDataService = ServiceLocator.Get<GameDataService>();
            _inventory = new Inventory(this, gameDataService);
        }
        else
        {
            _inventory = new Inventory(this, _characterInfo);
        }
    }

    private void AddHealth(bool isPlayer)
    {
        if (isPlayer == true)
        {
            IReadOnlyGameDataService gameDataService = ServiceLocator.Get<GameDataService>();
            _health = new Health(this, _characterInfo, gameDataService);
        }
        else
        {
            _health = new Health(this, _characterInfo);
        }

        _health.Died += OnDied;
    }
    #endregion InitializationMethods

    public void SetInput(Vector3 movementInput, Vector3 rotationInput,
        bool isAttacking, bool isReloading,
        int numericWeaponIndex, int prevNextWeaponIndex)
    {
        _movement.SetInput(movementInput, rotationInput);
        _inventory.SetInput(isAttacking, isReloading,
            numericWeaponIndex, prevNextWeaponIndex);
    }

    public void GetCure(int points)
    {
        _health.GetCure(points);
    }

    public void GetDamage(int damage, DamageType damageType, Character attacker)
    {
        _health.GetDamage(damage, damageType, attacker);
    }

    private void OnDied(Character attacker)
    {
        _isAlive = false;
        _movement.Die();
    }

    private void FixedUpdate()
    {
        if (_isAlive == true && _isInitialized == true)
        {
            _movement.OnTick();
            _inventory.OnTick();
        }
    }

    private void OnDestroy()
    {
        _health.Died -= OnDied;
    }
}
