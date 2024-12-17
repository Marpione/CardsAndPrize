using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class CardCellBase : MonoBehaviour, ICardCell
{
    [SerializeField]
    private CardData _cardData;
    public CardData CardData { get { return _cardData; } }

    [SerializeField]
    private Image _cardBackgroundImage;
    [SerializeField]
    private Image _cardIconImage;

    public void Initialize(CardData cardData)
    {
        _cardData = cardData;
        _cardIconImage.gameObject.SetActive(false);
        _cardIconImage.sprite = _cardData.CardIcon;
    }

    public void ShowCard()
    {
        _cardBackgroundImage.sprite = _cardData._cardFrontSprite;
        _cardIconImage.gameObject.SetActive(true);
    }

    public void HideCard()
    {
        _cardBackgroundImage.sprite = _cardData.CardBackSprite;
        _cardIconImage.gameObject.SetActive(false);
    }
}
