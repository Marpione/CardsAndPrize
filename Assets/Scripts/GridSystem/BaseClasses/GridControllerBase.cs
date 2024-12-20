using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public abstract class GridControllerBase<TData, TCell, TConfig, TProcessor> : MonoBehaviour, IGridController<TConfig, TData>
 where TCell : class, IGridCell<TData>
 where TConfig : GridConfig<TData>
{
    [SerializeField] protected TConfig _gridConfig;
    public TConfig GridConfig => _gridConfig;
    protected GridLayoutGroup GridLayout => GetComponent<GridLayoutGroup>();
    protected readonly List<TCell> Cells = new();


    [SerializeField] private float _cardScalePercentage = 0.9f;


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
            cell.Initialize(item);
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

        float targetAspectRatio = 0.6f; 

        float cardWidth, cardHeight;
        if (cardBaseWidth / cardBaseHeight > targetAspectRatio)
        {
            cardHeight = cardBaseHeight * _cardScalePercentage;
            cardWidth = cardHeight * targetAspectRatio;
        }
        else
        {
            cardWidth = cardBaseWidth * _cardScalePercentage;
            cardHeight = cardWidth / targetAspectRatio;
        }

        GridLayout.cellSize = new Vector2(cardWidth, cardHeight);
    }
    protected virtual void ClearBoard()
    {
        Cells.Clear();
    }
}
