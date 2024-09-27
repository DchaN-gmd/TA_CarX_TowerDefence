using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Projectile : MonoBehaviour, IPooledObject<Projectile>
{
    [Header("Parameters")]
    [SerializeField] private AttackActivateType _activateType;
    [SerializeField, Min(0)] protected int Damage = 10;

    [Header("Events")]
    [SerializeField] private UnityEvent<Projectile> _attacked;
    [SerializeField] public UnityEvent<Projectile> _lostTarget;

    protected Enemy Target;

    public event Action<Projectile> Release;

    public event UnityAction<Projectile> Attacked
    {
        add => _attacked.AddListener(value);
        remove => _attacked.RemoveListener(value);
    }

    public event UnityAction<Projectile> LostTarget
    {
        add => _lostTarget.AddListener(value);
        remove => _lostTarget.RemoveListener(value);
    }

    protected virtual void Update()
    {
        if (_activateType == AttackActivateType.Update) Attack();
    }

    public void SetTarget(Enemy target)
    {
        Target = target;

        Target.Release += OnTargetRelease;
    }

    public void Attack()
    {
        if (Target == null)
        {
            _lostTarget?.Invoke(this);
            return;
        }

        ProcessAttack();
    }

    protected abstract void ProcessAttack();
    protected void ReleaseSelf() => Release?.Invoke(this);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.SetDamage(Damage);
            _attacked?.Invoke(this);
        }
    }

    private void OnTargetRelease(Enemy enemy)
    {
        enemy.Release -= OnTargetRelease;
        Target = null;
    }
}