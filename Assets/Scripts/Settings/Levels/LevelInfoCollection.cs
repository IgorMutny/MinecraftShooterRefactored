using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Levels Collection")]
public class LevelInfoCollection : ScriptableObject
{
    [field: SerializeField] public LevelInfo[] Levels { get; private set; }

    public LevelInfo GetLevelById(int id)
    {
        foreach (var level in Levels)
        {
            if (level.Id == id)
            {
                return level;
            }
        }

        throw new Exception($"Level id {id} does not exist");
    }
}
