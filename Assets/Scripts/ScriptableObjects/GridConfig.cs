using System.Collections.Generic;
using UnityEngine;

public abstract class GridConfig<T> : ScriptableObject, IGridData<T>
{
    [SerializeField] private Vector2 _cellSize = new Vector2(100, 100);
    [SerializeField] private Vector2 _spacing = new Vector2(10, 10);
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private List<T> _items;

    public Vector2 CellSize => _cellSize;
    public Vector2 Spacing => _spacing;
    public GameObject CellPrefab => _cellPrefab;
    public List<T> Items => _items;
}
