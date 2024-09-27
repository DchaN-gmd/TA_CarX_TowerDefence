using UnityEngine;

public class SimpleMoveStrategy : MonoBehaviour, IMoveable
{
    public void Move(Transform selfTransform, Transform target, float speed)
    {
        if (target == null) return;

        var translation = target.position - selfTransform.position;

        if (translation.magnitude > speed)
        {
            translation = translation.normalized * (speed * Time.deltaTime);
        }
        selfTransform.Translate(translation);
    }
}