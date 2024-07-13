using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pro_Pooling : MonoBehaviour
{
    private Stack<GameObject> pro_Instances = new Stack<GameObject>();

    public void AddToStack(GameObject item)
    {
        pro_Instances.Push(item);
    }

    public void InstantiateProjectile(GameObject prefabReferenceInitializePro, float2 direction, LayerMask layerMask, float2 position)
    {
        if (pro_Instances.Count > 0)
        {
            var myObjec = pro_Instances.Pop();
            SetUpProjectileFromTemplate(myObjec, prefabReferenceInitializePro, direction, layerMask, position);
        }
        else
        {
            SetUpProjectile(Instantiate(prefabReferenceInitializePro, new Vector3(position.x, position.y, 0), quaternion.identity), 
                direction, layerMask, position);
        }
            
    }


    public void SetUpProjectileFromTemplate(GameObject projectile, GameObject template, float2 direction, LayerMask layerMask, float2 position)
    {
        Pro_Controller controller;
        Pro_Controller proControllerTemplate;

       projectile.TryGetComponent<Pro_Controller>(out controller);
        TryGetComponent<Pro_Controller>(out proControllerTemplate);

        if (controller == null)
            return;

        controller.ReconfigureProjectileData(proControllerTemplate.GetProProperties());
        controller.InitializePro(direction, layerMask);
    }

    public void SetUpProjectile(GameObject projectile, float2 direction, LayerMask layerMask, float2 position)
    {
        Pro_Controller controller;
        TryGetComponent<Pro_Controller>(out controller);

        if (controller == null)
            return;

        controller.InitializePro(direction, layerMask);
    }
}
