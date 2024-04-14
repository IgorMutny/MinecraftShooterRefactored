public class DelayBetweenWavesState : ILevelState
{
    private LevelStateMachine _stateMachine;
    private TimerWrapper _timer;

    public void Enter(LevelStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _timer = ServiceLocator.Get<TimerWrapper>();
        _timer.AddSignal(stateMachine.Level.DelayBetweenWaves / 2, ShowAd);
        _timer.AddSignal(stateMachine.Level.DelayBetweenWaves, BeginNewWave);
    }

    private void BeginNewWave()
    {
        _stateMachine.SetState(new WaveBeginningState());
    }

    private void ShowAd()
    {
        //ServiceLocator.Get<AdService>().ShowFullScreenAd();
    }
}
