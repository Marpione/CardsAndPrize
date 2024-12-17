using TMPro;
using UnityEngine;

public class UIScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private IScoreSystem _scoreSystem;

    [SerializeField] private ScoreManager _scoreManager;

    private void OnEnable() => _scoreManager.OnScoreChanged += UpdateScore;
    private void OnDisable() => _scoreManager.OnScoreChanged -= UpdateScore;

    public void Initialize(IScoreSystem scoreSystem)
    {
        _scoreSystem = scoreSystem;
        UpdateScore(_scoreSystem.Score);
    }

    private void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }
}