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
    [SerializeField, ReadOnly]
    private string _id;
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
    public string Id
    {
        get
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
            return _id;
        }
    }
    public Sprite CardIcon => _cardIcon;
    public Sprite CardClosedIcon => _cardClosedIcon;
    public float CardHideDelay => _cardHideDelay;
    public Sprite CardBackSprite => _cardBackSprite;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_id))
        {
            GenerateNewId();
        }
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(_id))
        {
            GenerateNewId();
        }
    }

    private void GenerateNewId()
    {
        _id = Guid.NewGuid().ToString();
    }
}
