using System.Collections.Generic;
using UnityEngine;

public class WaveBeginningState : ILevelState
{
    private LevelStateMachine _stateMachine;
    private CharacterCollection _characterCollection;
    private TimerWrapper _timer;
    private List<CharacterInfo> _enemiesToSpawn;

    public void Enter(LevelStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _characterCollection = ServiceLocator.Get<CharacterCollection>();
        _timer = ServiceLocator.Get<TimerWrapper>();
        _enemiesToSpawn = GetEnemiesList();

        ShowMessage();
        PlaySound();
        OnSignal();
    }

    private void ShowMessage()
    {
        MessageSender sender = ServiceLocator.Get<MessageSender>();
        int currentWave = _stateMachine.CurrentWave + 1;
        string text = $"<size=120%>����� <size=150%>{currentWave}<size=120%> ��������!";
        sender.ShowMessage(text, Color.white);
    }

    private void PlaySound()
    {
        AudioClip waveBegins = 
            ServiceLocator.Get<SettingsService>().Get<MiscObjectsCollection>().WaveBegins;

        ServiceLocator.Get<AudioService>().Play(waveBegins);
    }

    private void OnSignal()
    {
        if (_enemiesToSpawn.Count > 0)
        {
            CreateRandomEnemy();
            _timer.AddSignal(_stateMachine.Level.DelayBetweenEnemies, OnSignal);
        }
        else
        {
            _stateMachine.SetState(new WaveEndingState());
        }
    }

    private void CreateRandomEnemy()
    {
        int enemyIndex = Random.Range(0, _enemiesToSpawn.Count);
        CharacterInfo character = _enemiesToSpawn[enemyIndex];
        _characterCollection.CreateEnemy(character);
        _enemiesToSpawn.Remove(character);
    }

    private List<CharacterInfo> GetEnemiesList()
    {
        List<CharacterInfo> result = new List<CharacterInfo>();

        DefineWaveInfo(out WaveInfo waveInfo, out int additionalEnemies);

        foreach (WaveRecord record in waveInfo.Records)
        {
            for (int i = 0; i < record.Amount; i++)
            {
                result.Add(record.Character);
            }

            for (int i = 0; i < additionalEnemies; i++)
            {
                int rnd = Random.Range(0, waveInfo.Records.Length - 1);
                WaveRecord rndRecord = waveInfo.Records[rnd];
                result.Add(rndRecord.Character);
            }
        }

        return result;
    }

    private void DefineWaveInfo(out WaveInfo waveInfo, out int additionalEnemies)
    {
        LevelInfo level = _stateMachine.Level;
        int currentWave = _stateMachine.CurrentWave;

        if (currentWave < level.Waves.Length)
        {
            waveInfo = level.Waves[currentWave];
            additionalEnemies = 0;
        }
        else
        {
            waveInfo = level.Waves[level.Waves.Length - 1];
            int wavesExcess = currentWave - level.Waves.Length + 1;
            additionalEnemies = level.AdditionalEnemiesPerWave * wavesExcess;
        }
    }
}
