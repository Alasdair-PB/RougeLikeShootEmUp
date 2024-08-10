using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAction", menuName = "Actions/MultiAction")]
public class Multi_Act : E_Action
{
    [SerializeField] public E_Action[] actionList;

    public override bool StateIsComplete(E_Controller my_controller, float elapsedTime)
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
        actionList[0].SetUpFormations(my_controller);

    }

    private bool IsFinal(int actionIndex) => actionIndex >= actionList.Length - 1;

    public override void TakeAction(E_Controller my_controller, float elapsedTime, LayerMask layerMask, GlobalPooling pooling)
    {        
        var timeAtLastAction = elapsedTime - my_controller.GetActionTimeDirty();
        var actionIndex = my_controller.GetActionIndexDirty();

        if (actionList[actionIndex].IsComplete(my_controller, timeAtLastAction)){

            if (IsFinal(actionIndex))
            {
                // Should never really be called#
                Debug.Log("final called");
                my_controller.ReturnLastItemToStackTimeModified(elapsedTime);
                my_controller.ReturnLastItemToStackIndexModified(actionIndex);
                return;
            }
            else
            {
                actionIndex++;
                timeAtLastAction = 0;
                actionList[actionIndex].SetUp(my_controller);
                actionList[actionIndex].SetUpFormations(my_controller);
            }
        }

        var ex_ElapsedTime = my_controller.GetEx_ElapsedTime();
        actionList[actionIndex].TakeAction(my_controller, timeAtLastAction, layerMask, pooling);
        actionList[actionIndex].UpdateFormations(layerMask, my_controller, timeAtLastAction, pooling, ref ex_ElapsedTime);
        my_controller.SetEx_ElapsedTime(ex_ElapsedTime);


        if (timeAtLastAction == 0)
            my_controller.ReturnLastItemToStackTimeModified(elapsedTime);
        else
            my_controller.ReturnLastItemToStackTime();
        

        my_controller.ReturnLastItemToStackIndexModified(actionIndex);
    }
}
