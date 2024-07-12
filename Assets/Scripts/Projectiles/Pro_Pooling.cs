using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pro_Pooling : MonoBehaviour
{
    private List<Pro_Instance> pro_Instances = new List<Pro_Instance>();


    private struct Pro_Instance
    {
        public GameObject proObject;
    }

    public void InstantiateProjectile(GameObject prefabReference)
    {

    }
}
