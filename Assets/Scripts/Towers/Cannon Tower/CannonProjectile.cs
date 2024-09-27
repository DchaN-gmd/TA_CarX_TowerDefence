using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CannonProjectile : Projectile
{
    [SerializeField, Min(0)] private float _timeBeforeReturnToPool;

    private CannonCalculatorService _calculatorService;
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _calculatorService = ServiceLocator.Instance.Get<CannonCalculatorService>();
    }

    protected override void ProcessAttack()
    {
        transform.parent = null;
        RotateToEnemy();

        _rigidbody.velocity = transform.forward * _calculatorService.GetProjectileSpeedToInterceptPoint(Target.transform, transform, Target.Speed);

        if(_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(WaitingToReturn());
    }

    private void RotateToEnemy()
    {
        var interceptPoint = _calculatorService.GetInterceptPoint(Target.transform, transform, Target.Speed, transform.position);
        var rotate = Quaternion.LookRotation(interceptPoint - transform.position).eulerAngles;
        transform.localEulerAngles = new Vector3(-_calculatorService.AngleInDegrees, rotate.y, 0f);
    }

    private IEnumerator WaitingToReturn()
    {
        yield return new WaitForSeconds(_timeBeforeReturnToPool);
        ReleaseSelf();
    }
}
