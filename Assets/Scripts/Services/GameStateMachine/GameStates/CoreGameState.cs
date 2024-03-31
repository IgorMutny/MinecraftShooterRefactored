using UnityEngine.SceneManagement;

public class CoreGameState : IGameState
{
    private CoreGame _coreGame;
    private LevelInfo _level;

    public void Enter()
    {
        int sceneId = ServiceLocator.Get<GameDataService>().SelectedLevel;
        _level = 
            ServiceLocator.Get<SettingsService>()
            .Get<LevelInfoCollection>()
            .GetLevelById(sceneId);

        SceneManager.LoadScene(_level.SceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Exit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _coreGame.Destroy();
        _coreGame = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _level.SceneName)
        {
            _coreGame = new CoreGame(_level);
        }
    }
}
