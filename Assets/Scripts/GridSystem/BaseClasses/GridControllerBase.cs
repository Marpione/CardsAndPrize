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

    protected virtual void Start() => Initialize();

    public virtual void Initialize()
    {
        GridLayout.cellSize = GridConfig.CellSize;
        GridLayout.spacing = GridConfig.Spacing;

        foreach (var item in GridConfig.Items)
        {
            var cellObject = Instantiate(GridConfig.CellPrefab, transform);
            var cell = cellObject.GetInterfaceComponent<TCell>();
            cell.Initialize(item, _processor);
            Cells.Add(cell);
        }
    }
}
