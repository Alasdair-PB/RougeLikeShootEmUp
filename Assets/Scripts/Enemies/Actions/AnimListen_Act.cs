using UnityEngine;
using System;
namespace Enemies
{

    /// <summary>
    /// Plays a animation and then time event actions to specific points in the animation 
    /// </summary>

    [CreateAssetMenu(fileName = "AnimListen", menuName = "Actions/AnimListen_Act")]
    public class AnimListen_Act : E_Action
    {
        public Animation rigAnim; // Or sprite based idk or maybe even id
        public bool lockProjectilesToEvent; // Use default burst timings of projectiles or change to fire them only on events
        public AnimEventItem[] animEvtSchedule;
        public E_Action[] e_Actions; // This is not designed to be recursive

        [Serializable]
        public struct AnimEventItem  {
            public float triggerTime;
            public EventActions[] actions;
            public QuickActions[] action;
        }

        public struct QuickActions
        {
            public E_Action action;
            public float endTriggerTime; 
        }

        [Serializable] public enum EventActions { EndAction, FireProjectile, EnterParryState, ExitParryState }


        // Fo
        public void OnControllerAwake()
        {
            // Build animation graph?
        }

        // 
        public void OnActionCancelled(E_Controller my_controller)
        {
            my_controller.ClearActionStack();
            // May only be needed on this action => clear action stack of all items below this one
        }

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

            // Actions used on events will still fire projectiles!!
            if (!lockProjectilesToEvent)
                base.TakeAction(my_controller, elapsedTime, layerMask, pooling);
            else
            {
                    // 

            }
        }
    }
}
