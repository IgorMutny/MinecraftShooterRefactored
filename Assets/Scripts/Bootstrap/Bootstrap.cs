using DG.Tweening;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelInfoCollection _levelInfoCollection;
    [SerializeField] private CharacterInfoCollection _characterInfoCollection;
    [SerializeField] private ItemInfoCollection _itemInfoCollection;
    [SerializeField] private MiscObjectsCollection _objectInfoCollection;

    private void Awake()
    {
        Application.targetFrameRate = 100;

        GameDataService gameDataService = new GameDataService();
        ServiceLocator.Register(gameDataService);

        SettingsService settingsService = new SettingsService();
        settingsService.Register(_itemInfoCollection);
        settingsService.Register(_objectInfoCollection);
        settingsService.Register(_levelInfoCollection);
        settingsService.Register(_characterInfoCollection);
        ServiceLocator.Register(settingsService);

        AudioService audioService = new AudioService(_objectInfoCollection);
        ServiceLocator.Register(audioService);

        GameStateMachine gameStateMachine = new GameStateMachine();
        ServiceLocator.Register(gameStateMachine);
    }
}
