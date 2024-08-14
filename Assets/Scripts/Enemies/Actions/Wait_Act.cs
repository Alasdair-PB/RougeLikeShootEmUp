using UnityEngine;

namespace Enemies
{

    [CreateAssetMenu(fileName = "WaitAction", menuName = "Actions/WaitAction")]
    public class Wait_Act : E_Action
    {
        public float time;

        public override bool StateIsComplete(E_Controller my_controller, float elapsedTime) => elapsedTime > time;

        public override void SetUp(E_Controller my_controller) { }

        public override void TakeAction(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
        {
            base.TakeAction(my_controller, elapsedTime, layerMask, pooling);
        }
    }
}
