using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterCollection: IService
{
    private SpawnerCollection _spawnerCollection;

    private Character _player;
    private List<Character> _enemies = new List<Character>();

    public event Action PlayerDied;
    public event Action<Character> EnemyDied;

    public CharacterCollection(SpawnerCollection spawnerCollection)
    {
        _spawnerCollection = spawnerCollection;
    }

    public void Destroy()
    {
        _player = null;
        _enemies.Clear();
        _enemies = null;
        _spawnerCollection = null;
    }

    public Character CreatePlayer()
    {
        if (_player != null)
        {
            throw new Exception("Can't create new player: already exists");
        }

        CharacterInfoCollection characterInfos = 
            ServiceLocator.Get<SettingsService>().Get<CharacterInfoCollection>();
        CharacterInfo playerInfo = characterInfos.Characters[0];
        GameObject sample = playerInfo.Instance;

        Vector3 position = _spawnerCollection.GetPlayerSpawnerPosition();
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        _player = GameObject.Instantiate(sample, position, rotation)
            .GetComponent<Character>();
        _player.Initialize(playerInfo, true);

        return _player;
    }

    public void CreateEnemy(CharacterInfo character)
    {
        GameObject sample = character.Instance;

        Vector3 position = _spawnerCollection.GetRandomEnemySpawnerPosition();
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Character enemy =
            GameObject.Instantiate(sample, position, rotation).GetComponent<Character>();
        enemy.Initialize(character, false);
        _enemies.Add(enemy);
    }

    public void DestroyPlayer()
    {
        PlayerDied?.Invoke();
    }

    public void DestroyEnemy(Character enemy)
    {
        _enemies.Remove(enemy);
        EnemyDied?.Invoke(enemy);
    }

    public int GetEnemiesCount()
    {
        return _enemies.Count;
    }
}
