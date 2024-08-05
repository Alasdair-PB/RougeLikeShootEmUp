using System;
using Unity.Mathematics;
using UnityEngine;

// Add Namespace!!!!!!


public class Pro_Controller : MonoBehaviour
{
    [SerializeField] private Projectile proProperties; 
    private float2 yBounds = new float2(), xBounds = new float2 ();

    private float2 direction = new float2(1,0), nextPos = new float2();
    // Add options for homing in the future
    private float elapsedTime;
    private LayerMask projectileMask; //Maybe should be a tag?
    public Action<Pro_Controller> OnReturn; 

    public void ReconfigureProjectileData(Projectile proProperties) => this.proProperties = proProperties;
    public Projectile GetProProperties() => proProperties;
    public void SetBounds(float2 newXBounds, float2 newYBounds)
    {
        xBounds = newXBounds;
        yBounds = newYBounds;
    }

    // Called on instatiated projectile to setUp
    public void InitializePro(float2 direction, LayerMask layerMask)
    {
        elapsedTime = 0;
        this.projectileMask = layerMask;
        this.direction = direction;
        nextPos = new float2 (transform.position.x, transform.position.y);
    }

    private void OnEnable()
    {
        InitializePro(direction, projectileMask);
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        nextPos = proProperties.pattern.MoveInDirection(nextPos, direction, proProperties.speed, elapsedTime);

        if (nextPos.x < xBounds.x || nextPos.x > xBounds.y || nextPos.y < yBounds.x || nextPos.y > yBounds.y)
        {
            OnReturn?.Invoke(this);
            return;
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(nextPos.x, nextPos.y, 0);
    }
}
