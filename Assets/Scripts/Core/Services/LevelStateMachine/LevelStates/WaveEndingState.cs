using UnityEngine;

public class WaveEndingState : ILevelState
{
    private LevelStateMachine _stateMachine;
    private CharacterCollection _characterCollection;
    private TimerWrapper _timer;
    private TimerSignal _killAllEnemiesSignal;

    public void Enter(LevelStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _characterCollection = ServiceLocator.Get<CharacterCollection>();
        _characterCollection.EnemyDied += OnEnemyDied;
        _timer = ServiceLocator.Get<TimerWrapper>();
    }

    private void OnEnemyDied(Character enemy, Character attacker)
    { 
        if (_characterCollection.GetEnemiesCount() == 1)
        {
            _killAllEnemiesSignal = _timer.AddSignal(15f, KillAllEnemies);
        }

        if (_characterCollection.GetEnemiesCount() == 0)
        {
            _timer.RemoveSignal(_killAllEnemiesSignal);
            _characterCollection.EnemyDied -= OnEnemyDied;
            _characterCollection.ClearDeadEnemies();
            ShowMessage();

            _stateMachine.IncreaseCurrentWave();
            _stateMachine.SetState(new DelayBetweenWavesState());
        }
    }

    private void KillAllEnemies()
    {
        _characterCollection.KillAllEnemies();
    }

    private void ShowMessage()
    {
        MessageSender sender = ServiceLocator.Get<MessageSender>();
        int currentWave = _stateMachine.CurrentWave + 1;
        string text = $"<size=120%>¬ŒÀÕ¿ <size=150%>{currentWave}<size=120%> «¿¬≈–ÿ≈Õ¿!";
        sender.ShowMessage(text, Color.white);
    }
}
