using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterCollection: IService
{
    private SpawnerCollection _spawnerCollection;

    private List<Character> _spawnedEnemies = new List<Character>();

    public event Action<Character, Character> EnemyDied;

    public Character Player { get; private set; }

    public CharacterCollection(SpawnerCollection spawnerCollection)
    {
        _spawnerCollection = spawnerCollection;
    }

    public void Destroy()
    {
        Player = null;

        _spawnedEnemies.Clear();
        _spawnedEnemies = null;

        _spawnerCollection = null;
    }

    public Character CreatePlayer()
    {
        if (Player != null)
        {
            throw new Exception("Can't create new player: already exists");
        }

        CharacterInfoCollection characterInfos = 
            ServiceLocator.Get<SettingsService>().Get<CharacterInfoCollection>();
        CharacterInfo playerInfo = characterInfos.Characters[0];
        GameObject sample = playerInfo.Instance;

        Vector3 position = _spawnerCollection.GetPlayerSpawnerPosition();
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Player = GameObject.Instantiate(sample, position, rotation)
            .GetComponent<Character>();
        Player.Initialize(playerInfo, true);

        return Player;
    }

    public void CreateEnemy(CharacterInfo character)
    {
        GameObject sample = character.Instance;

        Vector3 position = _spawnerCollection.GetRandomEnemySpawnerPosition();
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Character enemy =
            GameObject.Instantiate(sample, position, rotation).GetComponent<Character>();
        enemy.Initialize(character, false);
        _spawnedEnemies.Add(enemy);
        enemy.Health.Died += (Character attacker) => OnEnemyDied(enemy, attacker);
    }

    private void OnEnemyDied(Character enemy, Character attacker)
    {
        enemy.Health.Died -= (Character attacker) => OnEnemyDied(enemy, attacker);
        EnemyDied?.Invoke(enemy, attacker);
    }

    public int GetEnemiesCount()
    {
        int result = 0;
        foreach (Character enemy in _spawnedEnemies)
        {
            if (enemy.IsAlive == true)
            {
                result += 1;
            }
        }

        return result;
    }

    public void ClearDeadEnemies()
    {
        foreach (Character enemy in _spawnedEnemies)
        {
            enemy.View.Dissolve();
            enemy.View.Dissolved += OnDissolved;
        }
    }

    private void OnDissolved(Character sender)
    {
        sender.View.Dissolved -= OnDissolved;
        _spawnedEnemies.Remove(sender);
        GameObject.Destroy(sender.gameObject);
    }
}
