using System;
using UnityEngine;

public class ScoreManager : ScriptableObject, IScoreSystem
{
    private int _score;
    public int Score => _score;
    public event Action<int> OnScoreChanged;

    private void OnEnable() => _score = 0;

    public void AddScore(int points)
    {
        _score += points;
        OnScoreChanged?.Invoke(_score);
    }
}