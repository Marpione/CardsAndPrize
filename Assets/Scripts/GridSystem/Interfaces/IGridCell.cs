public interface IGridCell<T1>
{
    void Initialize(T1 value1);
}

public interface IGridCell<T1, T2>
{
    void Initialize(T1 value1, T2 value2);
}
