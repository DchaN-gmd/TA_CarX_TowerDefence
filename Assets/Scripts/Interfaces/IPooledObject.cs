using System;

public interface IPooledObject<T>
{
    public event Action<T> Release;
}