using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public Transform Head { get; private set; }
    [field: SerializeField] public Transform UpperObstacleChecker { get; private set; }
    [field: SerializeField] public Transform LowerObstacleChecker { get; private set; }
    [field: SerializeField] public Transform AttackPoint { get; private set; }
    [field: SerializeField] public Transform GroundChecker { get; private set; }

    private bool _isInitialized = false;

    private CharacterInfo _info;
    private AI _ai;

    public bool IsPlayer { get; private set; }
    public bool IsAlive { get; private set; }

    public Movement Movement { get; private set; }
    public Inventory Inventory { get; private set; }
    public Health Health { get; private set; }
    public AppliedEffects AppliedEffects { get; private set; }
    public CharacterView View { get; private set; }
    public DropInfo DropInfo { get; private set; }

    public void Initialize(CharacterInfo characterInfo, bool isPlayer)
    {
        _info = characterInfo;
        DropInfo = characterInfo.LootChance;

        IsAlive = true;
        IsPlayer = isPlayer;

        View = new CharacterView(this, _info.Audio);

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
                Movement = new WalkingMovement(this, _info); break;
            case MovementType.Flying:
                Movement = new FlyingMovement(this, _info); break;
        }
    }

    private void AddInventory()
    {
        if (IsPlayer == true)
        {
            IReadOnlyGameDataService gameDataService = ServiceLocator.Get<GameDataService>();
            Inventory = new Inventory(this, gameDataService);
        }
        else
        {
            Inventory = new Inventory(this, _info);
        }
    }

    private void AddHealth()
    {
        if (IsPlayer == true)
        {
            IReadOnlyGameDataService gameDataService = ServiceLocator.Get<GameDataService>();
            Health = new Health(this, _info, gameDataService);
        }
        else
        {
            Health = new Health(this, _info);
        }

        Health.Died += OnDied;
        Health.Damaged += OnDamaged;
    }

    private void AddAppliedEffects()
    {
        AppliedEffects = new AppliedEffects(this);
        AppliedEffects.SpeedMultiplierChanged += OnSpeedMultiplierChanged;
        AppliedEffects.DamageMultiplierChanged += OnDamageMultiplierChanged;
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
        Movement.SetInput(movementInput, rotationInput);
        Inventory.SetInput(isAttacking, isReloading,
            numericWeaponIndex, prevNextWeaponIndex);
    }

    public void OnDamaged(int damage, Character attacker)
    {
        View.OnDamaged(damage);
    }

    private void OnDied(Character attacker)
    {
        Movement.OnDied();
        Inventory.OnDied();
        View.OnDied();
        AppliedEffects.OnDied();
        IsAlive = false;
    }

    private void OnSpeedMultiplierChanged(float multiplier)
    {
        Inventory.ChangeWeaponSpeed(multiplier);
        View.ChangeSpeed(multiplier);
    }

    private void OnDamageMultiplierChanged(float multiplier)
    {
        Inventory.ChangeWeaponDamage(multiplier);
    }

    private void FixedUpdate()
    {
        if (IsAlive == true && _isInitialized == true)
        {
            Movement.OnTick();
            AppliedEffects.OnTick();

            if (_ai != null)
            {
                _ai.OnTick();
            }
        }
    }

    private void OnDestroy()
    {
        Health.Died -= OnDied;
        Health.Damaged -= OnDamaged;
        AppliedEffects.SpeedMultiplierChanged -= OnSpeedMultiplierChanged;
        AppliedEffects.DamageMultiplierChanged -= OnDamageMultiplierChanged;
        View.Destroy();
    }
}
