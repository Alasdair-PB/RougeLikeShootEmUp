using System;
using Unity.Mathematics;
using UnityEngine;

// Add Namespace!!!!!!


public class Pro_Controller : MonoBehaviour
{
    [SerializeField] private Projectile proProperties;
    private C_OnContact c_OnContact;
    private float2 yBounds = new float2(), xBounds = new float2 ();

    private float2 direction = new float2(1,0), nextPos = new float2();
    // Add options for homing in the future
    private float elapsedTime;
    private LayerMask projectileMask; 
    public Action<Pro_Controller> OnReturn;

    public int GetLayerMask() => projectileMask;
    public void ReconfigureProjectileData(Projectile proProperties) => this.proProperties = proProperties;
    public Projectile GetProProperties() => proProperties;
    public void SetBounds(float2 newXBounds, float2 newYBounds)
    {
        xBounds = newXBounds;
        yBounds = newYBounds;
    }

    private void Awake()
    {
        c_OnContact = GetComponent<C_OnContact>();
    }

    // Called on instatiated projectile to setUp
    public void InitializePro(float2 direction, LayerMask layerMask)
    {
        elapsedTime = 0;
        c_OnContact.SetLayerMask(layerMask);
        this.direction = direction;
        nextPos = new float2 (transform.position.x, transform.position.y);
    }

    private void OnEnable()
    {
        InitializePro(direction, projectileMask);
        c_OnContact.OnCollision += Return;
    }

    private void OnDisable()
    {
        c_OnContact.OnCollision -= Return;
    }

    private void Return() => OnReturn?.Invoke(this);


    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        nextPos = proProperties.pattern.MoveInDirection(nextPos, direction, proProperties.speed, elapsedTime);

        if (nextPos.x < xBounds.x || nextPos.x > xBounds.y || nextPos.y < yBounds.x || nextPos.y > yBounds.y)
        {
            Return();
            return;
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3(nextPos.x, nextPos.y, 0);
    }
}
