using UnityEngine;

public class CannonCalculatorService : IService
{
    private readonly float _gravity = Physics.gravity.y;

    private float _angleInDegrees;
    private Transform _enemyEndPoint;

    public CannonCalculatorService(float angleInDegrees, Transform enemyEndPoint)
    {
        _angleInDegrees = angleInDegrees;
        _enemyEndPoint = enemyEndPoint;
    }

    public float AngleInDegrees => _angleInDegrees;

    public float CalculateProjectileSpeed(Transform target, Transform selfTransform) =>
        CalculateProjectileSpeed(target.position, selfTransform);

    public float CalculateProjectileSpeed(Vector3 targetPosition, Transform selfTransform)
    {
        var fromTo = targetPosition - selfTransform.position;
        var fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        var x = fromToXZ.magnitude;
        var y = fromTo.y;

        var v2 = (_gravity * x * x) /
                 (2 * (y - Mathf.Tan(_angleInDegrees * Mathf.Deg2Rad) * x) * Mathf.Pow(Mathf.Cos(_angleInDegrees * Mathf.Deg2Rad), 2));
        var v = Mathf.Sqrt(Mathf.Abs(v2));
        return v;
    }

    public Vector3 GetInterceptPoint(Transform target, Transform selfTransform, float targetSpeed, Vector3 projectilePosition)
    {
        var toTarget = target.position - projectilePosition;

        var distanceToTarget = toTarget.magnitude;
        var timeToIntercept = distanceToTarget / CalculateProjectileSpeed(target, selfTransform);
        var targetDirection = (_enemyEndPoint.position - target.position).normalized * targetSpeed;

        var futureTargetPosition = target.position + (targetDirection.normalized * targetSpeed * timeToIntercept);
        return futureTargetPosition;
    }

    public float GetProjectileSpeedToInterceptPoint(Transform target, Transform projectileTransform, float targetSpeed)
    {
        var interceptPoint = GetInterceptPoint(target, projectileTransform, targetSpeed, projectileTransform.position);

        return CalculateProjectileSpeed(interceptPoint, projectileTransform);
    }
}