using UnityEngine;

public class GuidedProjectile : Projectile
{
    [SerializeField, Min(0)] private float _speed = 0.2f;

    protected override void ProcessAttack()
    {
        var translation = Target.transform.position - transform.position;
        var speed = _speed * Time.deltaTime;

        if (translation.magnitude > speed)
        {
            translation = translation.normalized * speed;
        }
        transform.Translate(translation);
    }
}