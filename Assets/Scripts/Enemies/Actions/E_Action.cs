using UnityEngine;
using System.Collections.Generic;

namespace Enemies
{
    public abstract class E_Action : ScriptableObject
    {
        public Formation_Base[] projectileFormations;

        public bool mustComplete;
        public bool completeOnFormationEnd;
        public float priority;
        public abstract void SetUp(E_Controller my_controller);
        public virtual void TakeAction(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
        {
            var ex_ElapsedTime = my_controller.GetEx_ElapsedTime();
            UpdateFormations(layerMask, my_controller, elapsedTime, pooling, ref ex_ElapsedTime);
            my_controller.SetEx_ElapsedTime(ex_ElapsedTime);
        }

        public abstract bool StateIsComplete(E_Controller my_controller, float elapsedTime);

        public bool IsComplete(E_Controller my_controller, float elapsedTime)
        {
            if (!completeOnFormationEnd)
                return StateIsComplete(my_controller, elapsedTime);

            var isComplete = true;
            var ex_elapsedTime = my_controller.GetEx_ElapsedTime();

            Stack<int>[] burstCounts = my_controller.GetBurstCounter();
            for (int i = 0; i < projectileFormations.Length; i++)
            {
                if (!projectileFormations[i].IsComplete(ref burstCounts[i]))
                {
                    isComplete = false;
                    break;
                }
            }

            return isComplete;
        }


        public void SetUpFormations(E_Controller my_controller)
        {
            my_controller.ClearBurstCounter();
            Stack<int>[] burstCounts = my_controller.GetBurstCounter();

            for (int i = 0; i < projectileFormations.Length; i++)
            {
                if (burstCounts[i] == null)
                    burstCounts[i] = new Stack<int>();
                my_controller.SetEx_ElapsedTime(new Stack<float>(new float[] { 0.0f }));
                var exElaspedTime = my_controller.GetEx_ElapsedTime();
                burstCounts[i] = projectileFormations[i].SetUp(ref burstCounts[i], ref exElaspedTime);

            }

            my_controller.SetBurstCounter(burstCounts);
        }

        public void UpdateFormations(LayerMask layerMask, E_Controller my_controller, float elapsedTime, GlobalPooling pooling, ref Stack<float> ex_elapsedTime)
        {
            Stack<int>[] burstCounts = my_controller.GetBurstCounter();

            for (int i = 0; i < projectileFormations.Length; i++)
            {
                if (projectileFormations[i].IsComplete(ref burstCounts[i]))
                {
                    continue;
                }
                burstCounts[i] = projectileFormations[i].UpdateFormation(layerMask, ref burstCounts[i], elapsedTime, pooling,
                    my_controller.GetCurrentPosition(), ref ex_elapsedTime, false);
            }

            my_controller.SetBurstCounter(burstCounts);
        }
    }
}
