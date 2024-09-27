using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : SerializedMonoBehaviour, IDamagable, IPooledObject<Enemy>
{
    [Header("References")]
    [SerializeField] private EnemyData _data;
    [OdinSerialize] private IMoveable _moveable;

    [Header("Events")]
    [SerializeField] private UnityEvent<Enemy> _dieing;

    private Transform _target;
    private bool _isMoving;

    public int Hp { get; private set; }
    public float Speed => _data.Speed;

    public event Action<Enemy> Release;

    public event UnityAction<Enemy> Dieing
    {
        add => _dieing.AddListener(value);
        remove => _dieing.RemoveListener(value);
    }

    protected virtual void Update()
    {
        if (_isMoving)
        {
            Move();
            CheckDistance();
        }
    }

    public void SetDamage(int value)
    {
        if (value < 0) throw new ArgumentException("Damage can't be less then 0");

        Hp -= value;

        if (Hp < 0) Hp = 0;
        if (Hp == 0) Die();
    }

    public void UpdateHealth() => Hp = _data.MaxHp;

    public void SetTarget(Transform target) => _target = target;

    public void SetMove(bool value) => _isMoving = value;

    protected void Die()
    {
        _dieing?.Invoke(this);
        Release?.Invoke(this);
        ProcessDie();
    }

    protected abstract void ProcessDie();

    private void Move() => 
        _moveable.Move(transform, _target, _data.Speed);

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, _target.position) <= GameConfig.ReachDistance)
        {
            _isMoving = false;
            Release?.Invoke(this);
        }
    }
}