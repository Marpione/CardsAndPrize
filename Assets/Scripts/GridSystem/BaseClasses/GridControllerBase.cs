using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public abstract class GridControllerBase<TData, TCell, TConfig, TProcessor> : MonoBehaviour, IGridController<TConfig, TData>
 where TCell : class, IGridCell<TData, TProcessor>
 where TConfig : GridConfig<TData>
{
    [SerializeField] protected TConfig _gridConfig;
    protected TProcessor _processor;
    public TConfig GridConfig => _gridConfig;
    protected GridLayoutGroup GridLayout => GetComponent<GridLayoutGroup>();
    protected readonly List<TCell> Cells = new();


    [SerializeField] private float _cardScalePercentage = 0.9f;


    protected virtual void Start() => Initialize();

    public virtual void Initialize()
    {
        GridLayout.cellSize = GridConfig.CellSize;
        GridLayout.spacing = GridConfig.Spacing;
        GridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        GridLayout.constraintCount = GridConfig.GridSize.x;
        ScaleCards();

        foreach (var item in GridConfig.Items)
        {
            var cellObject = Instantiate(GridConfig.CellPrefab, transform);
            var cell = cellObject.GetInterfaceComponent<TCell>();
            cell.Initialize(item, _processor);
            Cells.Add(cell);
        }
    }

    protected void ScaleCards()
    {
        var rect = GetComponent<RectTransform>();
        float containerWidth = rect.rect.width;
        float containerHeight = rect.rect.height;

        float cardBaseWidth = (containerWidth - (GridConfig.GridSize.x - 1) * GridLayout.spacing.x) / GridConfig.GridSize.x;
        float cardBaseHeight = (containerHeight - (GridConfig.GridSize.y - 1) * GridLayout.spacing.y) / GridConfig.GridSize.y;

        // Aspect ratio of the card (width:height)
        float targetAspectRatio = 0.6f; // Adjust this value to match your card's desired aspect ratio

        float cardWidth, cardHeight;
        if (cardBaseWidth / cardBaseHeight > targetAspectRatio)
        {
            // Width is too large for desired ratio, base on height
            cardHeight = cardBaseHeight * _cardScalePercentage;
            cardWidth = cardHeight * targetAspectRatio;
        }
        else
        {
            // Height is too large for desired ratio, base on width
            cardWidth = cardBaseWidth * _cardScalePercentage;
            cardHeight = cardWidth / targetAspectRatio;
        }

        GridLayout.cellSize = new Vector2(cardWidth, cardHeight);
    }
}
