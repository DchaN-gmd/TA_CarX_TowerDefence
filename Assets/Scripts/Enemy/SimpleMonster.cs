using UnityEngine;

public class SimpleMonster : Enemy
{
    protected override void ProcessDie() => 
        gameObject.SetActive(false);
}