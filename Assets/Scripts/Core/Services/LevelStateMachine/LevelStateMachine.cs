using UnityEngine;

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

    public void OnTick()
    {
        _currentState.OnTick();
    }

    public void IncreaseCurrentWave()
    {
        _currentWave += 1;
    }

    public void Destroy()
    {
        _currentState = null;
    }
}
