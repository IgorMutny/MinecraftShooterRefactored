using UnityEngine;

public class DelayBetweenWavesState : ILevelState
{
    private LevelStateMachine _stateMachine;
    private float _timer;

    public void Enter(LevelStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _timer = stateMachine.Level.DelayBetweenWaves;
    }

    public void OnTick()
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0)
        {
           //_stateMachine.SetState(new WaveBeginningState());
        }
    }
}
