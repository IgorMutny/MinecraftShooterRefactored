using UnityEngine;

public class WaveEndingState : ILevelState
{
    private LevelStateMachine _stateMachine;
    private CharacterCollection _characterCollection;

    public void Enter(LevelStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _characterCollection = ServiceLocator.Get<CharacterCollection>();
        _characterCollection.EnemyDied += OnEnemyDied;
    }

    private void OnEnemyDied(Character enemy, Character attacker)
    { 
        if (_characterCollection.GetEnemiesCount() <= 3)
        {
            _characterCollection.EnemyDied -= OnEnemyDied;
            _characterCollection.ClearDeadEnemies();
            ShowMessage();
            PlaySound();

            _stateMachine.IncreaseCurrentWave();
            _stateMachine.SetState(new DelayBetweenWavesState());
        }
    }

    private void ShowMessage()
    {
        MessageSender sender = ServiceLocator.Get<MessageSender>();
        int currentWave = _stateMachine.CurrentWave + 1;
        string text = $"<size=120%>бнкмю <size=150%>{currentWave}<size=120%> гюбепьемю!";
        sender.ShowMessage(text, Color.white);
    }

    private void PlaySound()
    {
        AudioClip waveEnds =
            ServiceLocator.Get<SettingsService>().Get<MiscObjectsCollection>().WaveEnds;

        ServiceLocator.Get<AudioService>().Play(waveEnds);
    }
}
