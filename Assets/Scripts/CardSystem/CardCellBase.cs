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


    [SerializeField]
    private Transform _container;
    [SerializeField]
    private Image _cardBackgroundImage;
    [SerializeField]
    private Image _cardIconImage;


    private bool _isMatched;
    private CardMatchProcessor _matchProcessor;

    public void Initialize(CardData cardData, CardMatchProcessor cardMatchProcessor)
    {
        _cardData = cardData;
        _matchProcessor = cardMatchProcessor;
        HideCard();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isMatched) return;

        if (!IsProcessing && _matchProcessor != null)
        {
            IsProcessing = true;
            ShowCard();
            _matchProcessor.ProcessCard(this);
            Debug.Log($"Card being Processed {CardData.CardName} isProcessing {IsProcessing}");
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
        _isMatched = true;
        PlayMatchAnimation();
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

    private void DisableCard()
    {
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0;
    }
}
