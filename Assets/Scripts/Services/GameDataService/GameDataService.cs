using System;

public class GameDataService : IService, IReadOnlyGameDataService
{
    private GameDataProvider _gameDataProvider;
    private GameData _gameData;
    private AudioService _audioService;

    public float SoundVolume => _gameData.SoundVolume;
    public float MusicVolume => _gameData.MusicVolume;
    public float Sensitivity => _gameData.Sensitivity;
    public int Gold => _gameData.Gold;
    public int Diamonds => _gameData.Diamonds;
    public int SelectedLevel => _gameData.SelectedLevel;

    private bool _savesEnabled = true;

    public GameDataService()
    {
        _gameDataProvider = new GameDataProvider();
        _gameData = _gameDataProvider.Load();
    }

    private void Save()
    {
        if (_savesEnabled == true)
        {
            _gameDataProvider.Save(_gameData);
        }
    }

    public void AddGold(int value)
    {
        if (value < 0)
        {
            throw new Exception("Can't add gold: value should be positive");
        }

        _gameData.Gold += value;
        Save();
    }

    public void SubtractGold(int value)
    {
        if (value > _gameData.Gold)
        {
            throw new Exception("Can't substract gold: value is too large");
        }

        _gameData.Gold -= value;
        Save();
    }

    public void AddDiamonds(int value)
    {
        if (value < 0)
        {
            throw new Exception("Can't add diamonds: value should be positive");
        }

        _gameData.Diamonds += value;
        Save();
    }

    public void SubtractDiamonds(int value)
    {
        if (value > _gameData.Diamonds)
        {
            throw new Exception("Can't substract diamonds: value is too large");
        }

        _gameData.Diamonds -= value;
        Save();
    }

    public void AddItem(int id)
    {
        if (_gameData.Items[id] == true)
        {
            throw new Exception("Can't add item: player already has it");
        }

        _gameData.Items[id] = true;
        Save();
    }

    public void RemoveItem(int id)
    {
        if (_gameData.Items[id] == false)
        {
            throw new Exception("Can't remove item: player doesn't have it");
        }

        _gameData.Items[id] = false;
        Save();
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
        Save();
    }

    public void AddLevel(int id)
    {
        if (_gameData.Levels[id] == true)
        {
            throw new Exception("Can't add level: it is already added");
        }

        _gameData.Levels[id] = true;
        Save();
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
        Save();
    }

    public void SetMusicVolume(float value)
    {
        _gameData.MusicVolume = value;
        Save();
    }

    public void SetSensitivity(float value)
    {
        _gameData.Sensitivity = value;
        Save();
    }
}
