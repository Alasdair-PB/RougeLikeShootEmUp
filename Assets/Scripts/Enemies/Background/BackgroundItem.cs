using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// This item does not have any data and is moved in a loop from the pooling system along the pattern set for that!!
public class BackgroundItem : MonoBehaviour 
{
    public Action<BackgroundItem> OnReturn;

}
