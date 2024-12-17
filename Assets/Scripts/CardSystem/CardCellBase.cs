using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class CardCellBase : MonoBehaviour, ICardCell
{
    [SerializeField]
    private CardData _cardData;
    public CardData CardData { get { return _cardData; } }

    public bool IsProcessing { get; set; }
    public bool IsMatched {  get; set; }

    [SerializeField]
    private Image _cardBackgroundImage;
    [SerializeField]
    private Image _cardIconImage;

    private CardMatchProcessor _matchProcessor;

    public void Initialize(CardData cardData, CardMatchProcessor cardMatchProcessor)
    {
        _cardData = cardData;
        _cardIconImage.gameObject.SetActive(false);
        _cardIconImage.sprite = _cardData.CardIcon;
        _matchProcessor = cardMatchProcessor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsMatched) return;

        if (!IsProcessing && _matchProcessor != null)
        {
            ShowCard();
            _matchProcessor.ProcessCard(this);
        }
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
