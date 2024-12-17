using UnityEngine;
using UnityEngine.UI;

public class UIPlayButton : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _onGameStart;
    [SerializeField] private VoidEventChannel _onGameOver;
    [SerializeField] private Button _playButton;

    private CanvasGroup canvasGroup;
    protected CanvasGroup CanvasGroup => canvasGroup??= GetComponent<CanvasGroup>();

    private void Awake()
    {
        _playButton.onClick.AddListener(() => _onGameStart.RaiseEvent());
    }

    private void OnEnable()
    {
        _onGameStart.OnEventRaised += HideButton;
        _onGameOver.OnEventRaised += ShowButton;
    }
    private void OnDisable()
    {
        _onGameStart.OnEventRaised -= HideButton;
        _onGameOver.OnEventRaised -= ShowButton;
    }

    private void ShowButton()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;
    }

    private void HideButton()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;
    }
}