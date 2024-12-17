using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveLoadManager
{
    private const string SAVE_KEY = "card_match_save";

    public static void SaveGame(List<ICardCell> cells)
    {
        var saveData = new GameSaveData
        {
            originalCards = cells.Select(c => c.CardData).ToList(),
            matchedPairs = cells.Where(c => c.IsMatched)
                .Select(c => new CardMatchData { cardId = c.CardData.Id.ToString() })
                .Distinct()
                .ToList()
        };

        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(saveData));
    }

    public static GameSaveData LoadGame() =>
        JsonUtility.FromJson<GameSaveData>(PlayerPrefs.GetString(SAVE_KEY));
}