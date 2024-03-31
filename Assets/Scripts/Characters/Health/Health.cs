using System;
using UnityEngine;

public class Health : IReadOnlyHealth
{
    private Character _character;
    private CharacterInfo _characterInfo;
    private bool _isAlive = true;
    private int _maxHealth;
    private int _health;
    private int _minHealthPoisoned = 50;
    private float _physicalDefence;
    private float _magicalDefence;
    private Resurrector _resurrector;

    public int MaxHealth => _maxHealth;
    public bool CanBeResurrected => _resurrector.CanBeResurrected;
    public float Defence => _physicalDefence;

    public event Action<Character> Died;
    public event Action<Character> Attacked;
    public event Action<int> Cured;
    public event Action<int> Damaged;
    public event Action Resurrected;

    public Health(Character character, CharacterInfo characterInfo)
    {
        _character = character;
        _characterInfo = characterInfo;

        _maxHealth = _characterInfo.Health;
        _health = _maxHealth;
        _physicalDefence = 0;
        _magicalDefence = 0;
    }

    public Health(Character character,
        CharacterInfo characterInfo,
        IReadOnlyGameDataService gameDataService)
    {
        _character = character;
        _characterInfo = characterInfo;

        _maxHealth = _characterInfo.Health;
        _health = _maxHealth;
        _physicalDefence = 0;
        _magicalDefence = 0;

        SetDefence(gameDataService);
        _resurrector = new Resurrector(this, (GameDataService)gameDataService);
    }

    private void SetDefence(IReadOnlyGameDataService gameDataService)
    {
        ItemInfoCollection itemInfoCollection =
            ServiceLocator.Get<SettingsService>().Get<ItemInfoCollection>();

        ArmorInfo bestArmor = null;

        foreach (ItemInfo item in itemInfoCollection.Armors)
        {
            if (gameDataService.HasItem(item.Id) == true)
            {
                ArmorInfo armor = (ArmorInfo)item;
                bestArmor = armor;
            }
        }

        if (bestArmor != null)
        {
            _physicalDefence = bestArmor.PhysicalDefence;
            _magicalDefence = bestArmor.MagicalDefence;
        }
    }

    public void GetCure(int points)
    {
        if (_isAlive == false || points <= 0)
        {
            return;
        }

        _health += points;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }

        Cured?.Invoke(_health);
    }

    public void GetDamage(int damage, DamageType damageType, Character attacker)
    {
        if (damage <= 0)
        {
            return;
        }

        ReduceHealth(damage, damageType);

        if (attacker != null)
        {
            Attacked?.Invoke(attacker);
        }

        Damaged?.Invoke(_health);

        if (_health <= 0)
        {
            _health = 0;

            if (CanBeResurrected == true)
            {
                _resurrector.Resurrect();
            }
            else
            {
                _isAlive = false;
                Died?.Invoke(attacker);
            }
        }
    }

    private void ReduceHealth(int damage, DamageType damageType)
    {
        if (damageType == DamageType.Physical)
        {
            damage = (int)Mathf.Round(damage * (1f - _physicalDefence));
            _health -= damage;
        }

        if (damageType == DamageType.Magical)
        {
            damage = (int)Mathf.Round(damage * (1f - _magicalDefence));
            _health -= damage;
        }

        if (damageType == DamageType.Poison)
        {
            damage = (int)Mathf.Round(damage * (1f - _magicalDefence));
            if (_health - damage >= _minHealthPoisoned)
            {
                _health -= damage;
            }
        }
    }

    public void OnResurrected()
    {
        Resurrected?.Invoke();
    }
}
