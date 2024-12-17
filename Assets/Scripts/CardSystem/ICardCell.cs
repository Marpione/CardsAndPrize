using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardCell : IGridCell<CardData, CardMatchProcessor>
{
    public CardData CardData { get; }
    bool IsProcessing { get; set; }
    bool IsMatched { get; set; }
    public void ShowCard();
    public void HideCard();
}
