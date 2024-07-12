using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAction", menuName = "Actions/MultiAction")]
public class Multi_Act : E_Action
{
    [SerializeField] public E_Action[] actionList;

    public override bool IsComplete(E_Controller my_controller, float elapsedTime)
    {
        int index = my_controller.GetActionIndexDirty();
        var tempBool = (index != actionList.Length - 1) ? false : actionList[index].IsComplete(my_controller, elapsedTime);
        my_controller.ReturnLastItemToStackIndex();
        return tempBool;
    }

    public override void SetUp(E_Controller my_controller)
    {
        my_controller.PushActionIndex(0);
        my_controller.PushActionTime(0);
        actionList[0].SetUp(my_controller);
    }

    private bool IsFinal(int actionIndex) => actionIndex >= actionList.Length - 1;

    public override void TakeAction(E_Controller my_controller, float elapsedTime)
    {
        var actionIndex = my_controller.GetActionIndexDirty();
        var timeAtLastAction = elapsedTime - my_controller.GetActionTimeDirty();

        if (actionList[actionIndex].IsComplete(my_controller, timeAtLastAction)){

            if (IsFinal(actionIndex))
                return;
            else
            {
                actionIndex++;
                timeAtLastAction = 0;
                actionList[actionIndex].SetUp(my_controller);
            }
        }
        actionList[actionIndex].TakeAction(my_controller, timeAtLastAction);

        if (timeAtLastAction == 0)
            my_controller.ReturnLastItemToStackTimeModified(elapsedTime);
        else
            my_controller.ReturnLastItemToStackTime();

        my_controller.ReturnLastItemToStackIndexModified(actionIndex);
    }
}
