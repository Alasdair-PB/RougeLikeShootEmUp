using UnityEngine;
using Unity.Mathematics;
namespace Enemies
{

    [CreateAssetMenu(fileName = "ChargeAction", menuName = "Actions/ChargeAction")]
    public class Charge_Act : E_Action
    {
        public PatternBase pattern;
        public float time;
        public float2 chargePositionOffset;

        public override bool StateIsComplete(E_Controller my_controller, float elapsedTime)
        {
            var targetPos = my_controller.GetTargetPosition();
            return my_controller.IsAtPosition(my_controller.GetCurrentPosition(), targetPos);
        }

        public override void SetUp(E_Controller my_controller)
        {
            my_controller.SetTargetPosition(my_controller.GetCurrentPosition() + chargePositionOffset);
        }

        public override void TakeAction(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
        {

            var targetPos = my_controller.GetTargetPosition();
            var nextPos = pattern.LerpToPosition(my_controller.GetCurrentPosition(), targetPos, time, elapsedTime);
            my_controller.SetNextPosition(nextPos);

            base.TakeAction(my_controller, elapsedTime, layerMask, pooling);

        }
    }
}
