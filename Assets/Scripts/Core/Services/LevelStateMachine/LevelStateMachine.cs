public class LevelStateMachine: IService
{
    private LevelInfo _level;
    private int _currentWave;
    private ILevelState _currentState;

    public LevelInfo Level => _level;
    public int CurrentWave => _currentWave;

    public LevelStateMachine(LevelInfo level)
    {
        _level = level;
        _currentWave = 0;

        SetState(new DelayBetweenWavesState());
    }

    public void SetState(ILevelState state)
    {
        _currentState = state;
        _currentState.Enter(this);
    }

    public void IncreaseCurrentWave()
    {
        _currentWave += 1;

        if (_currentWave == _level.WaveToOpenNextLevel && _level.NextLevelId != 0)
        {
            ServiceLocator.Get<GameDataService>().AddLevel(_level.NextLevelId);
        }
    }

    public void Destroy()
    {
        _currentState = null;
    }
}
