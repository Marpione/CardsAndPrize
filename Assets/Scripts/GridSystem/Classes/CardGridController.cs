using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardGridController : GridControllerBase<CardData, ICardCell, CardGridConfig, CardMatchProcessor>
{
    private CardMatchProcessor _matchProcessor = new();


    void OnDisable() => SaveLoadManager.SaveGame(Cells);

    public override void Initialize()
    {
        var saveData = SaveLoadManager.LoadGame();
        if (saveData != null)
        {
            InitializeFromSave(saveData);
        }
        else
        {
            InitializeRandomGrid();
        }
    }

    private void InitializeFromSave(GameSaveData saveData)
    {
        ConfigureGridLayout();
        ScaleCards();
        InstantiateCards(saveData.originalCards);
        RestoreGameState(saveData);
    }

    private void InitializeRandomGrid()
    {
        ConfigureGridLayout();
        ScaleCards();

        int totalCards = GridConfig.GridSize.x * GridConfig.GridSize.y;
        var availableCards = new List<CardData>();
        while (availableCards.Count < totalCards / 2)
        {
            availableCards.AddRange(GridConfig.Items);
        }

        var selectedCards = availableCards.Take(totalCards / 2)
            .SelectMany(card => new[] { card, card })
            .ToList();

        ShuffleList(selectedCards);
        InstantiateCards(selectedCards);
    }

    private void ConfigureGridLayout()
    {
        GridLayout.cellSize = GridConfig.CellSize;
        GridLayout.spacing = GridConfig.Spacing;
        GridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        GridLayout.constraintCount = GridConfig.GridSize.x;
    }

    private void InstantiateCards(List<CardData> cards)
    {
        foreach (var card in cards)
        {
            var cellObject = Instantiate(GridConfig.CellPrefab, transform);
            var cell = cellObject.GetComponent<ICardCell>();
            cell.Initialize(card, _matchProcessor);
            Cells.Add(cell);
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

    private void RestoreGameState(GameSaveData saveData)
    {
        foreach (var cell in Cells)
        {
            var matchData = saveData.matchedPairs.FirstOrDefault(m =>
                m.cardId == cell.CardData.Id.ToString());

            if (matchData != null)
            {
                cell.IsMatched = true;
                cell.ShowCard();
                cell.IsProcessing = false;
                cell.DisableCard();
            }
        }
    }
}