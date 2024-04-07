using CoreUIElements;
using UnityEngine;

public class CoreGame
{
    private TimerWrapper _timer;
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

        CreateMessageSender();

        CreateLevelStateMachine();
        CreateTimer();

        Cursor.lockState = CursorLockMode.Locked;
    }

    #region InitializationMethods
    private void CreatePauseHandler()
    {
        _pauseHandler = new PauseHandler();
        ServiceLocator.Register(_pauseHandler);
        _pauseHandler = ServiceLocator.Get<PauseHandler>();
        _pauseHandler.PauseSwitched += OnPauseSwitched;
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
        _coreUi.ResumeButtonClicked += () => _pauseHandler.Resume();
        _coreUi.ExitButtonClicked += ExitToMainMenu;
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
        _timer = new TimerWrapper();
        _timer.Tick += OnTick;
        ServiceLocator.Register(_timer);
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

    private void OnPauseSwitched(bool isPaused)
    {
        _coreUi.SwitchInGameMenu(isPaused);
    }

    private void ExitToMainMenu()
    {
        GameStateMachine gameStateMachine = ServiceLocator.Get<GameStateMachine>();
        gameStateMachine.SetState(new MetaGameState());
    }

    public void Destroy()
    {
        _timer.Tick -= OnTick;
        ServiceLocator.Unregister<TimerWrapper>();
        _timer = null;

        GameObject.Destroy(_input.gameObject);

        _coreUi.ResumeButtonClicked -= () => _pauseHandler.Resume();
        _coreUi.ExitButtonClicked -= ExitToMainMenu;
        GameObject.Destroy(_coreUi.gameObject);

        ServiceLocator.Unregister<MessageSender>();
        _messageSender = null;

        _characterCollection.Destroy();
        ServiceLocator.Unregister<CharacterCollection>();
        _characterCollection = null;

        _lootCollection.Destroy();
        ServiceLocator.Unregister<LootCollection>();
        _lootCollection = null;

        _levelStateMachine.Destroy();
        ServiceLocator.Unregister<LevelStateMachine>();
        _levelStateMachine = null;

        _pauseHandler.PauseSwitched -= OnPauseSwitched;
        _pauseHandler.Destroy();
        ServiceLocator.Unregister<PauseHandler>();
        _pauseHandler = null;
    }
}
