using UnityEngine;
using System;
namespace Enemies
{
    [CreateAssetMenu(fileName = "AnimListen", menuName = "Actions/AnimListen_Act")]
    public class AnimListen_Act : E_Action
    {
        public Animation rigAnim; // Or sprite based idk
        public bool lockProjectilesToEvent; // Use default burst timings of projectiles or change to fire them only on events
        public animEvtItem[] animEvtSchedule;
        public E_Action[] e_Actions; // This is not designed to be recursive

        [Serializable]
        public struct animEvtItem  {
            public float triggerTime;
            public evtActions[] actions;
        }

        [Serializable] public enum evtActions { EndAction, FireProjectile, EnterParryState, ExitParryState }

        public override bool StateIsComplete(E_Controller my_controller, float elapsedTime)
        {
            var animator = my_controller.GetComponent<Animator>(); // Get through my_controller to ensure animator exists
            //if (animation is not finished)
            // return false

            var targetPos = my_controller.GetTargetPosition();
            // remove added animation from animator to clean up

            return true;
        }
   
        public override void SetUp(E_Controller my_controller)
        {
            var animator = my_controller.GetComponent<Animator>(); // Get through my_controller to ensure animator exists
            // add animation to animator and set animation to play



            my_controller.SetTargetPosition(my_controller.GetCurrentPosition());
        }

        public override void TakeAction(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
        {
            var animator = my_controller.GetComponent<Animator>(); // Get through my_controller to ensure animator exists

            /*
            if (//is not playing the chosen animation)
                return;

            if ()
            */

            if (!lockProjectilesToEvent)
                base.TakeAction(my_controller, elapsedTime, layerMask, pooling);
            else
            {
                    // 

            }
        }

        private void EndAction(E_Controller my_controller)
        {

        }

        private void FireProjectile(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
        {

        }

        private void EnterParryState(E_Controller my_controller)
        {

        }

        private void ExitParryState(E_Controller my_controller)
        {

        }
    }
}
