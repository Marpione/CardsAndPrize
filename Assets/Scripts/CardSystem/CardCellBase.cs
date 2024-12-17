using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class CardCellBase : MonoBehaviour, ICardCell, IPointerClickHandler
{
    [SerializeField]
    private CardData _cardData;
    public CardData CardData { get { return _cardData; } }

    public bool IsProcessing { get; set; }

    private CanvasGroup _canvasGroup;
    protected CanvasGroup CanvasGroup => _canvasGroup??= GetComponent<CanvasGroup>();

    public bool IsMatched { get; set; }

    [SerializeField]
    private Transform _container;
    [SerializeField]
    private Image _cardBackgroundImage;
    [SerializeField]
    private Image _cardIconImage;

    [SerializeField]
    private CardMatchProcessor _matchProcessor;

    public void Initialize(CardData cardData)
    {
        _cardData = cardData;
        HideCard();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsMatched) return;

        if (!IsProcessing && _matchProcessor != null)
        {
            IsProcessing = true;
            ShowCard();
            _matchProcessor.ProcessCard(this);
        }
    }

    public void ShowCard()
    {
        _cardBackgroundImage.sprite = _cardData._cardFrontSprite;
        _cardIconImage.sprite = _cardData.CardIcon;
    }

    public void HideCard()
    {
        _cardBackgroundImage.sprite = _cardData.CardBackSprite;
        _cardIconImage.sprite = _cardData.CardClosedIcon;
    }

    public void LockCard()
    {
        IsMatched = true;
        PlayMatchAnimation();
    }
    public void DisableCard()
    {
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0;
    }

    //This could go into another class for animations but I want to keep it simple for the demo
    private void PlayMatchAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(_container.DOPunchScale(Vector2.one * 0.2f, 0.4f, 5))
            .Append(_container.DOScale(Vector2.zero, 0.3f))
            .AppendCallback(DisableCard);
    }
}
