using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardCell : IGridCell<CardData>
{
    public CardData CardData { get; }
    public void ShowCard();
    public void HideCard();
}
