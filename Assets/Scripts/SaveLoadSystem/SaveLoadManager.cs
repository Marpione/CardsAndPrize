using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveLoadManager
{
    public const string SAVE_KEY = "card_match_save";

    public static void SaveGame(List<ICardCell> cells, ScoreManager scoreManager)
    {
        var saveData = new GameSaveData
        {
            Score = scoreManager.Score,
            OriginalCards = cells.Select(c => c.CardData).ToList(),
            MatchedPairs = cells.Where(c => c.IsMatched)
                .Select(c => new CardMatchData
                {
                    cardId = c.CardData.Id.ToString(),
                    isMatched = true
                }).ToList()
        };

        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(saveData));
        PlayerPrefs.Save();
    }

    public static GameSaveData LoadGame()
    {
        var json = PlayerPrefs.GetString(SAVE_KEY);
        return !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<GameSaveData>(json) : null;
    }
}