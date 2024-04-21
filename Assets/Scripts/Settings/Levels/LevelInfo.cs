using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Level")]
public class LevelInfo : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int WaveToOpenNextLevel { get; private set; }
    [field: SerializeField] public int NextLevelId { get; private set; }
    [field: SerializeField] public string SceneName { get; private set; }
    [field: SerializeField] public WaveInfo[] Waves { get; private set; }
    [field: SerializeField] public float DelayBetweenWaves { get; private set; }
    [field: SerializeField] public float DelayBetweenEnemies { get; private set; }
    [field: SerializeField] public int AdditionalEnemiesPerWave { get; private set; }
}
