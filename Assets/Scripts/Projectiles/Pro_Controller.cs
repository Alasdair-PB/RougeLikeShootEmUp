using Unity.Mathematics;
using UnityEngine;

public class Pro_Controller : MonoBehaviour
{
    private Projectile proProperties; 
    private float2 direction, nextPos;
    // Add options for homing in the future
    private float elapsedTime;
    private LayerMask projectileMask; //Maybe should be a tag?

    // Reconfigure if projectile already present in pooling list to avoid instantiation
    public void ReconfigureProjectileData(Projectile proProperties)
    {
        this.proProperties = proProperties;
    }

    // Called on instatiated projectile to setUp
    public void InitializePro(float2 direction, LayerMask layerMask)
    {
        elapsedTime = 0;
        this.projectileMask = layerMask;
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        nextPos = proProperties.pattern.MoveInDirection(nextPos, direction, proProperties.speed, elapsedTime);
    }
}
