using CoreUIElements;
using UnityEngine;

public class CoreGame
{
    private Timer _timer;
    private PCInput _input;
    private CharacterCollection _characterCollection;
    private LootCollection _lootCollection;
    private LevelStateMachine _levelStateMachine;
    private CoreUI _coreUi;
    private PauseHandler _pauseHandler;
    private MessageSender _messageSender;

    private LevelInfo _level;

    public CoreGame(LevelInfo level)
    {
        _level = level;

        CreatePauseHandler();

        CreateCharacterCollection();
        CreateLootCollection();

        Character player = CreatePlayer();
        CoreCameras coreCameras = CreateCoreCameras(player);
        CreateUI(player, coreCameras);
        CreateInput(player);

        CreateLevelStateMachine();
        CreateTimer();
        CreateMessageSender();
    }

    #region InitializationMethods
    private void CreatePauseHandler()
    {
        _pauseHandler = new PauseHandler();
        ServiceLocator.Register(_pauseHandler);
    }

    private void CreateCharacterCollection()
    {
        SpawnerCollection spawnerCollection = GameObject.FindObjectOfType<SpawnerCollection>();
        _characterCollection = new CharacterCollection(spawnerCollection);
        ServiceLocator.Register(_characterCollection);
    }

    private void CreateLootCollection()
    {
        _lootCollection = new LootCollection();
        ServiceLocator.Register(_lootCollection);
    }

    private Character CreatePlayer()
    {
        Character player = _characterCollection.CreatePlayer();
        _lootCollection.SetPlayer(player);
        return player;
    }

    private CoreCameras CreateCoreCameras(Character player)
    {
        GameObject coreCamerasSample =
            ServiceLocator.Get<SettingsService>().Get<MiscObjectsCollection>().CoreCameras;
        Transform head = player.Head;
        CoreCameras coreCameras = GameObject.Instantiate(coreCamerasSample, head)
            .GetComponent<CoreCameras>();
        return coreCameras;
    }

    private void CreateUI(Character player, CoreCameras coreCameras)
    {
        GameObject uiSample =
            ServiceLocator.Get<SettingsService>().Get<MiscObjectsCollection>().CoreUI;
        Transform cameraTransform = coreCameras.UICamera.transform;
        _coreUi = GameObject.Instantiate(uiSample, cameraTransform).GetComponent<CoreUI>();
        _coreUi.Initialize(player);
    }

    private void CreateInput(Character player)
    {
        GameObject inputSample =
            ServiceLocator.Get<SettingsService>().Get<MiscObjectsCollection>().PCInput;
        _input = GameObject.Instantiate(inputSample).GetComponent<PCInput>();
        _input.Initialize(player);
    }

    private void CreateLevelStateMachine()
    {
        _levelStateMachine = new LevelStateMachine(_level);
        ServiceLocator.Register(_levelStateMachine);
    }

    private void CreateTimer()
    {
        GameObject timerSample =
            ServiceLocator.Get<SettingsService>().Get<MiscObjectsCollection>().Timer;
        _timer = GameObject.Instantiate(timerSample).GetComponent<Timer>();
        _timer.Tick += OnTick;
    }

    private void CreateMessageSender()
    {
        _messageSender = new MessageSender(_coreUi);
        ServiceLocator.Register(_messageSender);
    }

    #endregion InitializationMethods

    private void OnTick()
    {
        _levelStateMachine.OnTick();
    }

    public void Destroy()
    {
        _timer.Tick -= OnTick;
        GameObject.Destroy(_timer.gameObject);

        GameObject.Destroy(_input.gameObject);

        GameObject.Destroy(_coreUi.gameObject);

        ServiceLocator.Unregister<MessageSender>();
        _messageSender = null;

        ServiceLocator.Unregister<CharacterCollection>();
        _characterCollection = null;

        ServiceLocator.Unregister<LootCollection>();
        _lootCollection = null;

        _levelStateMachine.Destroy();
        ServiceLocator.Unregister<LevelStateMachine>();
        _levelStateMachine = null;

        _pauseHandler.Destroy();
        ServiceLocator.Unregister<PauseHandler>();
        _pauseHandler = null;
    }
}
