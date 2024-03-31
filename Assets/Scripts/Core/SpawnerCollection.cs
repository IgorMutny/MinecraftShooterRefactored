using UnityEngine;

public class SpawnerCollection : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawner;
    [SerializeField] private Transform[] _enemySpawners;

    public Vector3 GetPlayerSpawnerPosition()
    {
        return _playerSpawner.position;
    }

    public Vector3 GetRandomEnemySpawnerPosition()
    {
        int rnd = Random.Range(0, _enemySpawners.Length);
        Transform spawner = _enemySpawners[rnd];
        return spawner.position;
    }
}
