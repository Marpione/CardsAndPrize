using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardMatchProcessor : ScriptableObject
{
    private const string cardMatchingSoundId = "MatchingSound";
    private const string cardMissMatchingSoundId = "MissMatchingSound";

    private ICardCell _waitingCard;
    private readonly List<Task> _activeProcesses = new();
    private readonly object _lock = new();

    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private StringEventChannel _audioEvent;


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
        await Task.Delay(1000);


        bool isMatch = first.CardData.Id == second.CardData.Id;


        if (isMatch)
        {
            first.LockCard();
            second.LockCard();
            _scoreManager.AddScore(1);
            _audioEvent.RaiseEvent(cardMatchingSoundId);
        }

        
        if (!isMatch)
        {
            first.HideCard();
            second.HideCard();
            _audioEvent.RaiseEvent(cardMissMatchingSoundId);
        }

        first.IsProcessing = second.IsProcessing = false;
    }

    private void CleanupCompletedTasks()
    {
        _activeProcesses.RemoveAll(t => t.IsCompleted);
    }
}
