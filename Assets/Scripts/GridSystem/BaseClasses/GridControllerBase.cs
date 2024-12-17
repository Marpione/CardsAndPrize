using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public abstract class GridControllerBase<TData, TCell, TConfig> : MonoBehaviour, IGridController<TConfig, TData>
   where TCell : Component, IGridCell<TData>
   where TConfig : GridConfig<TData>
{
    [SerializeField] private TConfig _gridConfig;
    public TConfig GridConfig => _gridConfig;
    private GridLayoutGroup GridLayout => GetComponent<GridLayoutGroup>();
    protected readonly List<TCell> Cells = new();

    private void Start() => Initialize();

    public virtual void Initialize()
    {
        GridLayout.cellSize = GridConfig.CellSize;
        GridLayout.spacing = GridConfig.Spacing;

        foreach (var item in GridConfig.Items)
        {
            var cellObject = Instantiate(GridConfig.CellPrefab, transform);
            var cell = cellObject.GetComponent<TCell>();
            cell.Initialize(item);
            Cells.Add(cell);
        }
    }
}
