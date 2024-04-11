using UnityEngine;
using MetaUIElements;
using System;

public class MetaGame
{
    private GameDataService _gameDataService;
    private ItemInfoCollection _itemsCollection;
    private MetaUI _metaUi;

    public MetaGame()
    {
        _gameDataService = ServiceLocator.Get<GameDataService>();

        SettingsService settingsService = ServiceLocator.Get<SettingsService>();
        _itemsCollection = settingsService.Get<ItemInfoCollection>();

        CreateUI(settingsService);
    }

    private void CreateUI(SettingsService settingsService)
    {
        GameObject metaUiSample = settingsService.Get<MiscObjectsCollection>().MetaUI;
        _metaUi = GameObject.Instantiate(metaUiSample).GetComponent<MetaUI>();
        _metaUi.Initialize(this, _itemsCollection, _gameDataService);
    }

    public void TryBuyItem(ItemInfo item)
    {
        if (_gameDataService.HasItem(item.Id) == true)
        {
            return;
        }

        if (_gameDataService.Gold < item.PriceInGold)
        {
            return;
        }

        if (_gameDataService.Diamonds < item.PriceInDiamonds)
        {
            return;
        }

        if (item.PriceInDiamonds < 0 || item.PriceInGold < 0)
        {
            throw new Exception("Price can't be less than zero");
        }

        _gameDataService.AddItem(item.Id);
        _gameDataService.SubtractGold(item.PriceInGold);
        _gameDataService.SubtractDiamonds(item.PriceInDiamonds);

        _metaUi.Reload();
    }

    public void TrySelectLevel(LevelInfo level)
    {
        if (_gameDataService.IsLevelOpen(level.Id) == false)
        {
            return;
        }

        if (_gameDataService.SelectedLevel == level.Id)
        {
            return;
        }

        _gameDataService.SetSelectedLevel(level.Id);
        _metaUi.Reload();
    }

    public void StartLevel()
    {
        ServiceLocator.Get<GameStateMachine>().SetState(new CoreGameState());
    }

    public void ChangeSoundVolume(float value)
    {
        _gameDataService.SetSoundVolume(value);
    }

    public void ChangeMusicVolume(float value)
    {
        _gameDataService.SetMusicVolume(value);
    }
}
