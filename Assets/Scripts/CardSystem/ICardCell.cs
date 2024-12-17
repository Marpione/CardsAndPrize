using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardCell : IGridCell<CardData, CardMatchProcessor>
{
    CardData CardData { get; }
    bool IsProcessing { get; set; }
    bool IsMatched { get; set; }
    void LockCard();
    void ShowCard();
    void HideCard();
    void DisableCard();
}
