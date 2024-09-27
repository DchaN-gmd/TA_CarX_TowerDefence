using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Data/Enemies", order = 51)]
public class EnemyData : ScriptableObject
{
    [SerializeField, Min(0)] private int _maxHp;
    [SerializeField, Min(0)] private float _speed;

    public int MaxHp => _maxHp;
    public float Speed => _speed;
}