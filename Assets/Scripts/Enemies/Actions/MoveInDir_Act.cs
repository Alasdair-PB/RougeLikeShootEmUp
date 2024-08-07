using UnityEngine;
using Unity.Mathematics;

[CreateAssetMenu(fileName = "DirectionalMoveAction", menuName = "Actions/DirectionalMoveAction")]
public class MoveInDir_Act : E_Action
{
    public PatternBase pattern;
    public float time, speed;
    public float2 direction;

    public override bool IsComplete(E_Controller my_controller, float elapsedTime) => elapsedTime > time;
    public override void SetUp(E_Controller my_controller) { }
    public override void TakeAction(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
    {
        base.TakeAction(my_controller, elapsedTime, layerMask, pooling);
        var nextPos = pattern.MoveInDirection(my_controller.GetCurrentPosition(), direction, speed, elapsedTime);
        my_controller.SetNextPosition(nextPos);
    }
}
