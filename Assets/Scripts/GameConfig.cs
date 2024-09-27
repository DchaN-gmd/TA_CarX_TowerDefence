using UnityEngine;

[CreateAssetMenu(fileName = "New Config", menuName = "Data/Config", order = 51)]
public sealed class GameConfig : ScriptableObject
{
    public static readonly float ReachDistance = 0.3f;
}