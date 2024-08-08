using UnityEngine;
using System.Collections.Generic;

public abstract class E_Action : ScriptableObject
{
    public Formation_Base[] projectileFormations; 

    public bool mustComplete;
    public float priority;
    public abstract void SetUp(E_Controller my_controller);
    public virtual void TakeAction(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
    {
        UpdateFormations(layerMask, my_controller, elapsedTime, pooling);
    }

    public abstract bool IsComplete(E_Controller my_controller, float elapsedTime);

    public void SetUpFormations(E_Controller my_controller)
    {
        my_controller.ClearBurstCounter();
        Stack<int>[] burstCounts = my_controller.GetBurstCounter();

        for (int i = 0; i < projectileFormations.Length; i++)
        {
            if (burstCounts[i] == null)
                burstCounts[i] = new Stack<int>();
            burstCounts[i] = projectileFormations[i].SetUp(burstCounts[i]);
        }

        my_controller.SetBurstCounter(burstCounts);
    }

    public void UpdateFormations(LayerMask layerMask, E_Controller my_controller, float elapsedTime, GlobalPooling pooling)
    {
        Stack<int>[] burstCounts = my_controller.GetBurstCounter();

        for (int i = 0; i < projectileFormations.Length; i++)
        {
            if (projectileFormations[i].IsComplete(burstCounts[i], elapsedTime, my_controller.GetCurrentPosition()))
                continue;
            burstCounts[i] = projectileFormations[i].UpdateFormation(layerMask, burstCounts[i], elapsedTime, pooling, my_controller.GetCurrentPosition());
        }

        my_controller.SetBurstCounter(burstCounts);
    }
}
