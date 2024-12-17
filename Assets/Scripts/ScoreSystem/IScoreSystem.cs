using System;

public interface IScoreSystem
{
    int Score { get; }
    event Action<int> OnScoreChanged;
    void AddScore(int points);
}
