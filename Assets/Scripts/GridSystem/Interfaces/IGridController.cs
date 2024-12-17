using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridController<TConfig, TData> where TConfig : GridConfig<TData>
{
    public TConfig GridConfig { get; }
    public void Initialize();
}
