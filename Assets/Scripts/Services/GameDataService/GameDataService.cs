using System;

public class GameDataService : IService, IReadOnlyGameDataService
{
    private GameDataProvider _gameDataProvider;
    private GameData _gameData;
    private AudioService _audioService;

    public float SoundVolume => _gameData.SoundVolume;
    public float MusicVolume => _gameData.MusicVolume;
    public int Gold => _gameData.Gold;
    public int Diamonds => _gameData.Diamonds;
    public int SelectedLevel => _gameData.SelectedLevel;

    public GameDataService()
    {
        _gameDataProvider = new GameDataProvider();
        _gameData = _gameDataProvider.Load();
    }

    public void Save()
    {
        _gameDataProvider.Save(_gameData);
    }

    public void AddGold(int value)
    {
        if (value < 0)
        {
            throw new Exception("Can't add gold: value should be positive");
        }

        _gameData.Gold += value;
    }

    public void SubtractGold(int value)
    {
        if (value > _gameData.Gold)
        {
            throw new Exception("Can't substract gold: value is too large");
        }

        _gameData.Gold -= value;
    }

    public void AddDiamonds(int value)
    {
        if (value < 0)
        {
            throw new Exception("Can't add diamonds: value should be positive");
        }

        _gameData.Diamonds += value;
    }

    public void SubtractDiamonds(int value)
    {
        if (value > _gameData.Diamonds)
        {
            throw new Exception("Can't substract diamonds: value is too large");
        }

        _gameData.Diamonds -= value;
    }

    public void AddItem(int id)
    {
        if (_gameData.Items[id] == true)
        {
            throw new Exception("Can't add item: player already has it");
        }

        _gameData.Items[id] = true;
    }

    public void RemoveItem(int id)
    {
        if (_gameData.Items[id] == false)
        {
            throw new Exception("Can't remove item: player doesn't have it");
        }

        _gameData.Items[id] = false;
    }

    public bool HasItem(int id)
    {
        return _gameData.Items[id];
    }

    public void SetSelectedLevel(int id)
    {
        if (_gameData.Levels[id] == false)
        {
            throw new Exception("Can't select level: it isn't open yet");
        }

        _gameData.SelectedLevel = id;
    }

    public void AddLevel(int id)
    {
        if (_gameData.Levels[id] == true)
        {
            throw new Exception("Can't add level: it is already added");
        }

        _gameData.Levels[id] = true;
    }

    public bool IsLevelOpen(int id)
    {
        return _gameData.Levels[id];
    }

    public void SetSoundVolume(float value)
    {
        _gameData.SoundVolume = value;

        if (_audioService == null)
        {
            _audioService = ServiceLocator.Get<AudioService>();
        }

        _audioService.SetVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        _gameData.MusicVolume = value;
    }
}
