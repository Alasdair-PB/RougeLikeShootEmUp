using System;
using Unity.Mathematics;
using UnityEngine;

public class BackgroundItem : MonoBehaviour 
{
    public Action<BackgroundItem> OnReturn;
    private float2 yBounds = new float2(), xBounds = new float2();

    public void SetBounds(float2 newXBounds, float2 newYBounds)
    {
        xBounds = newXBounds;
        yBounds = newYBounds;
    }

    private void Return() => OnReturn?.Invoke(this);

    private void FixedUpdate()
    {
        var nextPos = new float2(transform.position.x, transform.position.y);

        if (nextPos.x < xBounds.x || nextPos.x > xBounds.y || nextPos.y < yBounds.x || nextPos.y > yBounds.y)
        {
            Return();
            return;
        }
    }
}
