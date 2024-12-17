using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;


public class CardData : ScriptableObject
{
    [SerializeField]
    private string _cardName;
    private Guid _id = Guid.NewGuid();
    [SerializeField]
    private Sprite _cardIcon;
    [SerializeField]
    private Sprite _cardClosedIcon;
    [SerializeField]
    private float _cardHideDelay = 1f;
    [SerializeField]
    public Sprite _cardFrontSprite;
    [SerializeField]
    public Sprite _cardBackSprite;


    public string CardName => _cardName;
    public Guid Id => _id;
    public Sprite CardIcon => _cardIcon;
    public Sprite CardClosedIcon => _cardClosedIcon;
    public float CardHideDelay => _cardHideDelay;
    public Sprite CardBackSprite => _cardBackSprite;
}
