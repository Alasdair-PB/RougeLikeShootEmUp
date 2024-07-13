using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Projectile")]
public class Projectile : ScriptableObject
{
    public PatternBase pattern;
    public float speed;
}
