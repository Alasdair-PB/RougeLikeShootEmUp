using UnityEngine;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Formation")]
public class Formation : ScriptableObject
{
    public GameObject projectileObject;
    public float startDelay, burstTime, burstCount;
    public float[] angle;

    // May need randomness?
}
