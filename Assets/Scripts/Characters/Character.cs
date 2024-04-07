using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public Transform Head { get; private set; }
    [field: SerializeField] public Transform UpperObstacleChecker { get; private set; }
    [field: SerializeField] public Transform LowerObstacleChecker { get; private set; }
    [field: SerializeField] public Transform AttackPoint { get; private set; }
    [field: SerializeField] public Transform GroundChecker { get; private set; }

    private bool _isInitialized = false;

    private CharacterView _view;
    private CharacterInfo _info;
    private Movement _movement;
    private Inventory _inventory;
    private Health _health;
    private DropInfo _dropInfo;
    private AppliedEffects _appliedEffects;
    private AI _ai;

    public bool IsPlayer { get; private set; }
    public bool IsAlive { get; private set; }

    public Movement Movement => _movement;
    public Inventory Inventory => _inventory;
    public Health Health => _health;
    public AppliedEffects AppliedEffects => _appliedEffects;
    public CharacterView View => _view;
    public DropInfo DropInfo => _dropInfo;

    private void Awake()
    {
        _view = GetComponent<CharacterView>();
    }

    public void Initialize(CharacterInfo characterInfo, bool isPlayer)
    {
        _info = characterInfo;
        _dropInfo = characterInfo.LootChance;

        IsAlive = true;
        IsPlayer = isPlayer;

        AddMovement();
        AddInventory();
        AddHealth();
        AddAppliedEffects();
        AddAI();

        _isInitialized = true;
    }

    #region InitializationMethods
    private void AddMovement()
    {
        switch (_info.MovementType)
        {
            case MovementType.Walking: 
                _movement = new WalkingMovement(this, _info); break;
            case MovementType.Flying:
                _movement = new FlyingMovement(this, _info); break;
        }
    }

    private void AddInventory()
    {
        if (IsPlayer == true)
        {
            IReadOnlyGameDataService gameDataService = ServiceLocator.Get<GameDataService>();
            _inventory = new Inventory(this, gameDataService);
        }
        else
        {
            _inventory = new Inventory(this, _info);
        }
    }

    private void AddHealth()
    {
        if (IsPlayer == true)
        {
            IReadOnlyGameDataService gameDataService = ServiceLocator.Get<GameDataService>();
            _health = new Health(this, _info, gameDataService);
        }
        else
        {
            _health = new Health(this, _info);
        }

        _health.Died += OnDied;
        _health.Damaged += OnDamaged;
    }

    private void AddAppliedEffects()
    {
        _appliedEffects = new AppliedEffects(this);
        _appliedEffects.SpeedMultiplierChanged += OnSpeedMultiplierChanged;
        _appliedEffects.DamageMultiplierChanged += OnDamageMultiplierChanged;
    }

    private void AddAI()
    {
        if (IsPlayer == false)
        {
            _ai = new AI(this, _info);
        }
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

    public void OnDamaged(int damage, Character attacker)
    {
        _view.OnDamaged(damage);
    }

    private void OnDied(Character attacker)
    {
        _movement.OnDied();
        _inventory.OnDied();
        _view.OnDied();
        _appliedEffects.OnDied();
        IsAlive = false;
    }

    private void OnSpeedMultiplierChanged(float multiplier)
    {
        _inventory.ChangeWeaponSpeed(multiplier);
    }

    private void OnDamageMultiplierChanged(float multiplier)
    {
        _inventory.ChangeWeaponDamage(multiplier);
    }

    private void FixedUpdate()
    {
        if (IsAlive == true && _isInitialized == true)
        {
            _movement.OnTick();
            _inventory.OnTick();
            _appliedEffects.OnTick();

            if (_ai != null)
            {
                _ai.OnTick();
            }
        }
    }

    private void OnDestroy()
    {
        _health.Died -= OnDied;
        _health.Damaged -= OnDamaged;
        _appliedEffects.SpeedMultiplierChanged -= OnSpeedMultiplierChanged;
        _appliedEffects.DamageMultiplierChanged -= OnDamageMultiplierChanged;
    }
}
