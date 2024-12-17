using System.Collections.Generic;
using UnityEngine;

public interface IGridData<T>
{
    Vector2 CellSize { get; }
    Vector2 Spacing { get; }
    GameObject CellPrefab { get; }
    List<T> Items { get; }
}
