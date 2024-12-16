using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;


public class CardData : ScriptableObject
{
    [SerializeField]
    private string _cardName;
    private Guid _cardId;
    [SerializeField]
    private Sprite _cardVisual;


    public string CardName => _cardName;
    public Guid CardId => _cardId == null || _cardId == Guid.Empty ? (_cardId = Guid.NewGuid()) : _cardId;
    public Sprite CardVisual => _cardVisual;

}
