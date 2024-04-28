using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootCollection : IService
{
    private LootInfoCollection _infoCollection;
    private CharacterCollection _characterCollection;
    private MessageSender _messageSender;
    private AudioService _audioService;
    private GameDataService _gameDataService;

    private List<Loot> _loots = new List<Loot>();

    public event Action BalanceChanged;

    public LootCollection()
    {
        _characterCollection = ServiceLocator.Get<CharacterCollection>();
        _characterCollection.EnemyDied += OnEnemyDied;

        _infoCollection = ServiceLocator.Get<SettingsService>().Get<LootInfoCollection>();
        _audioService = ServiceLocator.Get<AudioService>();
        _gameDataService = ServiceLocator.Get<GameDataService>();
    }

    public void Destroy()
    {
        _characterCollection.EnemyDied -= OnEnemyDied;
    }

    public void AddDiamond()
    {
        _gameDataService.AddDiamonds(1);
        BalanceChanged?.Invoke();
    }

    private void OnEnemyDied(Character enemy, Character attacker)
    {
        if (attacker == null || attacker.IsPlayer == false)
        {
            return;
        }

        DropInfo dropInfo = enemy.DropInfo;
        Vector3 position = enemy.transform.position;

        IncreaseEnemiesKilled();
        TryAddGold(dropInfo);
        TryCreateLoot(dropInfo, position);
    }

    private void IncreaseEnemiesKilled()
    {
        _gameDataService.IncreaseEnemiesKilled();
    }

    private void TryAddGold(DropInfo dropInfo)
    {
        _gameDataService.AddGold(dropInfo.GoldAmount);
        BalanceChanged?.Invoke();
    }

    private void TryCreateLoot(DropInfo dropInfo, Vector3 position)
    {
        float rnd = Random.Range(0f, 1f);

        if (rnd < dropInfo.DiamondChance)
        {
            CreateLoot(_infoCollection.Diamond, position);
            return;
        }

        if (rnd < dropInfo.DiamondChance + dropInfo.MedKitChance)
        {
            CreateLoot(_infoCollection.MedKit, position);
            return;
        }

        if (rnd < dropInfo.DiamondChance + dropInfo.MedKitChance + dropInfo.PowerupChance)
        {
            int type = Random.Range(0, _infoCollection.Powerups.Length);
            CreateLoot(_infoCollection.Powerups[type], position);
            return;
        }
    }

    private void CreateLoot(LootInfo lootInfo, Vector3 position)
    {
        Loot loot = GameObject.Instantiate(lootInfo.Instance, position, Quaternion.identity)
            .GetComponent<Loot>();
        loot.Initialize(lootInfo, _infoCollection.Beam);
        loot.Picked += OnLootPicked;
        loot.Died += OnLootDied;
        _loots.Add(loot);
    }

    private void OnLootPicked(Loot loot)
    {
        if (_messageSender == null)
        {
            _messageSender = ServiceLocator.Get<MessageSender>();
        }

        _messageSender.ShowMessage(loot.Info.Message, loot.Info.Color);
        _audioService.Play(loot.Info.Clip);
        loot.Picked -= OnLootPicked;
        loot.Died -= OnLootDied;

        _loots.Remove(loot);
        GameObject.Destroy(loot.gameObject);
    }

    private void OnLootDied(Loot loot)
    {
        _loots.Remove(loot);
        GameObject.Destroy(loot.gameObject);
    }
}
