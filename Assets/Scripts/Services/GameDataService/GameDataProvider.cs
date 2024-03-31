using System;
using UnityEngine;

public class GameDataProvider
{
    public void Save(GameData gameData)
    {
        if (gameData != null)
        {
            string json = JsonUtility.ToJson(gameData);
            YG.YandexGame.savesData.Data = json;
            YG.YandexGame.SaveProgress();
        }
        else
        {
            throw new Exception("Game data can't be saved: game data is null");
        }
    }

    public GameData Load()
    {
        GameData gameData;
        string json = YG.YandexGame.savesData.Data;

        if (string.IsNullOrEmpty(json) == false)
        { 
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            gameData = new GameData();
            Save(gameData);
        }

        return gameData;
    }
}
