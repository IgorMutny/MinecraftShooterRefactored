using UnityEngine.SceneManagement;

public class MetaGameState : IGameState
{
    private readonly string _metaScene = "Meta";
    private MetaGame _metaGame;

    public void Enter()
    {
        SceneManager.LoadScene(_metaScene);
        SceneManager.sceneLoaded += OnSceneLoaded;   
    }

    public void Exit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _metaGame = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _metaScene)
        {
            _metaGame = new MetaGame();
        }
    }
}
