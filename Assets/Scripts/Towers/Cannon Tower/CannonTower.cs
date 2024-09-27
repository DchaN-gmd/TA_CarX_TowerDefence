using System;
using UnityEngine;

public class CannonTower : Tower
{
    [Header("Override Parameters")]
    [SerializeField] private bool _activateDebugCube;

    [Header("Override References")]
    [SerializeField] private Transform _shootPoint;

    private Quaternion _lookRotation;
    private CannonCalculatorService _calculatorService;
    
    private void OnValidate()
    {
        if (_shootPoint == null) throw new NullReferenceException($"{nameof(_shootPoint)} is null");
    }

    protected override void Awake()
    {
        base.Awake();

        _calculatorService = ServiceLocator.Instance.Get<CannonCalculatorService>();
    }

    protected override void Update()
    {
        if (Enemies.Count > 0)
        {
            base.Update();
        }
    }

    protected override void ProcessShoot(Projectile newProjectile)
    {
        base.ProcessShoot(newProjectile);

        newProjectile.transform.position = _shootPoint.position;
        newProjectile.Attack();
    }
}