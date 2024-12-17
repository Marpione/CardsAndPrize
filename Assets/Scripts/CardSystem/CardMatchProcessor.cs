using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardMatchProcessor
{
    private ICardCell _waitingCard;
    private readonly List<Task> _activeProcesses = new();
    private readonly object _lock = new();

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
        if (!isMatch)
        {
            first.HideCard();
            second.HideCard();
        }

        first.IsMatched = true;
        second.IsMatched = true;

        first.IsProcessing = second.IsProcessing = false;
    }

    private void CleanupCompletedTasks()
    {
        _activeProcesses.RemoveAll(t => t.IsCompleted);
    }
}
