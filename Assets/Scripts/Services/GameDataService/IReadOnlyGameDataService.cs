public interface IReadOnlyGameDataService
{
    public float SoundVolume { get; }
    public float MusicVolume { get; }
    public float Sensitivity { get; }
    public int Gold { get; }
    public int Diamonds { get; }
    public int SelectedLevel { get; }
    public bool HasItem(int id);
    public bool IsLevelOpen(int id);
}
