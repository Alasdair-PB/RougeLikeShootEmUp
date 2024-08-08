using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Formation", menuName = "Formations/Multi_Formation")]
public class Multi_Formation : Formation_Base
{
    public Formation_Base[] formations;


    public override bool IsComplete(Stack<int> occurredBursts, float elapsedTime, float2 position)
    {
        throw new System.NotImplementedException();
    }

    public override Stack<int> UpdateFormation(LayerMask layerMask, Stack<int> occurredBursts, float elapsedTime, GlobalPooling pooling, float2 position)
    {
        var my_occuredBursts = occurredBursts.Pop();

        //formations[];




        my_occuredBursts++;
        occurredBursts.Push(my_occuredBursts);

        return occurredBursts;
    }
}
