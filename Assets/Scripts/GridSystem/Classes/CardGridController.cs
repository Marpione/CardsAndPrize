using System.Collections.Generic;
using UnityEngine;

public class CardGridController : GridControllerBase<CardData, CardCell, CardGridConfig>
{
    public override void Initialize()
    {
        var pairedItems = new List<CardData>();
        foreach (var card in GridConfig.Items)
        {
            pairedItems.Add(card);
            pairedItems.Add(card);
        }

        ShuffleList(pairedItems);

        foreach (var item in pairedItems)
        {
            var cellObject = Instantiate(GridConfig.CellPrefab, transform);
            var cell = cellObject.GetComponent<CardCell>();
            cell.Initialize(item);
            Cells.Add(cell);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}