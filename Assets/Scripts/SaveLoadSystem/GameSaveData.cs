using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<CardData> originalCards = new();
    public List<CardMatchData> matchedPairs = new();
    public int totalPairs;
    public float playTime;
}