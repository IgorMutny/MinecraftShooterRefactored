using System;

[Serializable]
public class GameData
{
    public static readonly int ItemsCount = 16;
    public static readonly int LevelsCount = 4;

    public float SoundVolume;
    public float MusicVolume;
    public float Sensitivity;

    public int Gold;
    public int Diamonds;
    public int SelectedLevel;
    public bool[] Items = new bool[ItemsCount];
    public bool[] Levels = new bool[LevelsCount];
    public int EnemiesKilled;

    public GameData()
    {
        SoundVolume = 1f;
        MusicVolume = 1f;
        Sensitivity = 1000f;

        Gold = 0;
        Diamonds = 0;
        SelectedLevel = 1;

        Items[1] = true;
        Levels[1] = true;

        EnemiesKilled = 0;
    }
}
