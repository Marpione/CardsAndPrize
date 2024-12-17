using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardGridController : GridControllerBase<CardData, ICardCell, CardGridConfig, CardMatchProcessor>
{
    private ICardCell _firstSelectedCard;
    private List<Task> _matchProcessingTasks = new();
    private object _lockObject = new();
    private CardMatchProcessor _matchProcessor = new();


    public override void Initialize()
    {
        var pairedItems = GridConfig.Items.SelectMany(card => new[] { card, card }).ToList();
        ShuffleList(pairedItems);

        foreach (var item in pairedItems)
        {
            var cellObject = Instantiate(GridConfig.CellPrefab, transform);
            var cell = cellObject.GetComponent<ICardCell>();
            cell.Initialize(item, _matchProcessor);
            Cells.Add(cell);
        }

        foreach (var cell in Cells)
        {
            var button = cell.GetComponent<Button>();
            button.onClick.AddListener(() => OnCardClicked(cell));
        }
    }

    private void OnCardClicked(ICardCell clickedCard)
    {
        if (clickedCard.IsProcessing) return;
        clickedCard.ShowCard();

        lock (_lockObject)
        {
            if (_firstSelectedCard == null)
            {
                _firstSelectedCard = clickedCard;
                return;
            }

            var firstCard = _firstSelectedCard;
            _firstSelectedCard = null;

            var matchTask = ProcessMatch(firstCard, clickedCard);
            _matchProcessingTasks.Add(matchTask);
            CleanupCompletedTasks();
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private async Task ProcessMatch(ICardCell firstCard, ICardCell secondCard)
    {
        firstCard.IsProcessing = true;
        secondCard.IsProcessing = true;

        await Task.Delay(500);
        bool isMatch = firstCard.CardData.Id == secondCard.CardData.Id;

        if (!isMatch)
        {
            firstCard.HideCard();
            secondCard.HideCard();
        }

        firstCard.IsProcessing = false;
        secondCard.IsProcessing = false;
    }

    private void CleanupCompletedTasks()
    {
        _matchProcessingTasks.RemoveAll(t => t.IsCompleted);
    }
}