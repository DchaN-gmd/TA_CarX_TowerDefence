using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolMono<T> where T: MonoBehaviour
{
    public T Prefab { get; }
    public bool AutoExpand { get; set; }
    public Transform Parent { get; }

    private List<T> _pool;

    public PoolMono(T prefab, int count)
    {
        this.Prefab = prefab;
        Parent = null;

        CreatePool(count);
    }

    public PoolMono(T prefab, int count, Transform parent)
    {
        this.Prefab = prefab;
        this.Parent = parent;

        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(Prefab, Parent);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var targetElement in _pool)
        {
            if(!targetElement.gameObject.activeInHierarchy)
            {
                element = targetElement;
                element.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if(HasFreeElement(out T element))
        {
            return element;
        }

        if(AutoExpand)
        {
            return CreateObject(true);
        }

        throw new Exception($"There has no element of {typeof(T)} type");
    }

    public void AddElement(T element)
    {
        _pool.Add(element);
        element.gameObject.SetActive(false);
    }
}