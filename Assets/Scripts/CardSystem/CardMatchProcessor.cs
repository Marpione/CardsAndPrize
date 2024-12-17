using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardMatchProcessor : ScriptableObject
{
    private const string CardMatchingSoundId = "MatchingSound";
    private const string CardMissMatchingSoundId = "MissMatchingSound";
    private const string GameOverSoundId = "GameOverSound";

    private ICardCell _waitingCard;
    private readonly List<Task> _activeProcesses = new();
    private readonly object _lock = new();

    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private StringEventChannel _audioEvent;

    [SerializeField]
    private VoidEventChannel _onGameOver;
    private int _totalPairs;
    private int _matchedPairs;

    public void Initialize(int totalPairs)
    {
        _totalPairs = totalPairs;

        var savedData = SaveLoadManager.LoadGame();
        _matchedPairs = savedData?.MatchedPairs.Count / 2 ?? 0;
    }


    public void ProcessCard(ICardCell card)
    {
        lock (_lock)
        {
            if (_waitingCard == null)
            {
                _waitingCard = card;
                return;
            }

            var firstCard = _waitingCard;
            _waitingCard = null;
            StartMatchProcess(firstCard, card);
        }
    }

    private void StartMatchProcess(ICardCell first, ICardCell second)
    {
        var matchTask = ProcessMatch(first, second);
        _activeProcesses.Add(matchTask);
        CleanupCompletedTasks();
    }

    private async Task ProcessMatch(ICardCell first, ICardCell second)
    {
        first.IsProcessing = second.IsProcessing = true;
        await Task.Delay(1000);

        bool isMatch = first.CardData.Id == second.CardData.Id;
        first.IsMatched = second.IsMatched = isMatch;

        if (isMatch)
        {
            _scoreManager.AddScore(1);
            _matchedPairs++;
            first.LockCard();
            second.LockCard();
            _audioEvent.RaiseEvent(CardMatchingSoundId);

            if (_matchedPairs >= _totalPairs)
            {
                await Task.Delay(500);
                _onGameOver.RaiseEvent();
                _audioEvent.RaiseEvent(GameOverSoundId);
                //This is not good. I would find a better way with time.
                PlayerPrefs.DeleteKey(SaveLoadManager.SAVE_KEY);
            }
        }
        else
        {
            first.HideCard();
            second.HideCard();
            _audioEvent.RaiseEvent(CardMissMatchingSoundId);

        }

        first.IsProcessing = second.IsProcessing = false;
    }

    private void CleanupCompletedTasks()
    {
        _activeProcesses.RemoveAll(t => t.IsCompleted);
    }
}
