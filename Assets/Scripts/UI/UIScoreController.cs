using TMPro;
using UnityEngine;

public class UIScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private ScoreManager _scoreManager;

    private void OnEnable() => _scoreManager.OnScoreChanged += UpdateScore;
    private void OnDisable() => _scoreManager.OnScoreChanged -= UpdateScore;

    private void Start()
    {
        UpdateScore(_scoreManager.Score);
    }

    private void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }
}