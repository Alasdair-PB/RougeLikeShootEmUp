using UnityEngine;

public abstract class E_Action : ScriptableObject
{
    public Formation[] projectileFormations; 

    public bool mustComplete;
    public float priority;
    public abstract void SetUp(E_Controller my_controller);
    public abstract void TakeAction(E_Controller my_controller, float elapsedTime);
    public abstract bool IsComplete(E_Controller my_controller, float elapsedTime);
}
