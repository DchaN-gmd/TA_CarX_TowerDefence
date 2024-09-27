using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class SimplePool<T> : SerializedMonoBehaviour where T : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int _count;
    [SerializeField] private bool _autoExpand;
    [SerializeField] private Transform _parent;

    [Header("References")]
    [OdinSerialize] private T _prefab;

    private PoolMono<T> _pool;

    private void Awake()
    {
        _pool = new PoolMono<T>(_prefab, _count, _parent);
        _pool.AutoExpand = _autoExpand;
    }

    public T GetObject() => _pool.GetFreeElement();

    public void ReturnObjectToPool(T poolObject)
    {
        poolObject.transform.parent = _parent;
        poolObject.gameObject.SetActive(false);
        _pool.AddElement(poolObject);
    }
}