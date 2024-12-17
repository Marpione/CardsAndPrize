using System;
using UnityEngine;

public class ScoreManager : ScriptableObject, IScoreSystem
{
    private int _score;
    public int Score => _score;
    public event Action<int> OnScoreChanged;

    private void OnEnable() 
    {
        var saveData = SaveLoadManager.LoadGame();
        if (saveData != null)
            _score = saveData.Score;
        else _score = 0;
    }

    public void AddScore(int points)
    {
        _score += points;
        OnScoreChanged?.Invoke(_score);
    }
}