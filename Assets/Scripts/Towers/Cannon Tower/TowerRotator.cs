using UnityEngine;

[RequireComponent(typeof(Tower))]
public abstract class TowerRotator : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected bool _isActivate;
    [SerializeField, Min(0)] protected float RotateSpeed;

    protected Tower Tower;

    protected virtual void Awake() => Tower = GetComponent<Tower>();

    protected virtual void Update()
    {
        if(_isActivate) Rotate();
    }

    public void SetRotate(bool value) => _isActivate = value;

    protected abstract void Rotate();
}