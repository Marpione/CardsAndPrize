using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<CardData> OriginalCards = new();
    public List<CardMatchData> MatchedPairs = new();
    public int Score;
}