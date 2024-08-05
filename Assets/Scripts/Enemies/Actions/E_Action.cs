using UnityEngine;

public abstract class E_Action : ScriptableObject
{
    public Formation[] projectileFormations; 

    public bool mustComplete;
    public float priority;
    public abstract void SetUp(E_Controller my_controller);
    public abstract void TakeAction(E_Controller my_controller, float elapsedTime);
    public abstract bool IsComplete(E_Controller my_controller, float elapsedTime);

    public void SetUpFormations(E_Controller my_controller)
    {
        int[] newBurstCount = new int[projectileFormations.Length];
        my_controller.SetBurstCounter(newBurstCount);
    }

    public void UpdateFormations(LayerMask layerMask, E_Controller my_controller, float elapsedTime, GlobalPooling pooling)
    {
        int[] burstCounts = my_controller.GetBurstCounter();

        for (int i = 0; i < projectileFormations.Length; i++)
        {
            burstCounts[i] = projectileFormations[i].UpdateFormation(layerMask, burstCounts[i], elapsedTime, pooling, my_controller.GetCurrentPosition());
        }

        my_controller.SetBurstCounter(burstCounts);
    }
}
