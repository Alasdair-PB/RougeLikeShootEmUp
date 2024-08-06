using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class E_StateMachine : MonoBehaviour
{
    [SerializeField] private E_Action[] e_Actions;
    [SerializeField] private E_Action escapeAction;
    [SerializeField] private float escapeTime = 15, decisionTickMin = 1f, decisionTickMax = 4f;
    [SerializeField] private bool escapes; 

    private E_Controller controller;

    private float repeatPriorityModifier = 0, timeSinceLastDecision, timeAtSpawned, decisionTick = .5f;
    private int lastActionIndex, currentActionIndex;
    private bool escaping = false;

    private GlobalPooling pooling;
    [SerializeField] LayerMask layerMask;


    private void OnEnable()
    {
        timeSinceLastDecision = 0;
        timeAtSpawned = Time.time;
        escaping = false;

        var e_Action = e_Actions[currentActionIndex];
        SetNewAction(e_Action);
    }

    private void Awake()
    {
        escaping = false;
        pooling = FindAnyObjectByType<GlobalPooling>();
        controller = GetComponent<E_Controller>();

        //var e_Action = e_Actions[currentActionIndex];        
        //SetNewAction(e_Action);
    }

    protected void FixedUpdate()
    {
        if (!escaping && escapes && (Time.time - timeAtSpawned) > escapeTime)
            EscapeSetUp();

        var elapsedTime = Time.time - timeSinceLastDecision;

        if (escaping) {
            escapeAction.TakeAction(controller, elapsedTime, layerMask, pooling);
            if (escapeAction.IsComplete(controller, elapsedTime))
                EscapeSetUp();
            return;
        }

        var e_Action = e_Actions[currentActionIndex];

        if (e_Action.IsComplete(controller, elapsedTime)|| (!e_Action.mustComplete && elapsedTime > decisionTick))
        {
            SetNewAction(e_Action);
            elapsedTime = 0;
        }

        e_Action = e_Actions[currentActionIndex];

        if (e_Action != null)
            e_Action.TakeAction(controller, elapsedTime, layerMask, pooling);
    }

    private void EscapeSetUp()
    {
        escaping = true;
        timeSinceLastDecision = Time.time;
        escapeAction.SetUpFormations(controller);
        controller.ClearActionStack();
        controller.ClearActionTimeStack();
        escapeAction.SetUp(controller);
    }

    private void SetNewAction(E_Action e_Action)
    {        
        currentActionIndex = GetNextActionIndex();        
        e_Action = e_Actions[currentActionIndex];    
        
        e_Action.SetUpFormations(controller);
        decisionTick = Random.Range(decisionTickMin, decisionTickMax);
        timeSinceLastDecision = Time.time;

        controller.ClearActionStack();
        controller.ClearActionTimeStack();
        e_Action.SetUp(controller);
    }


    private float GetTotalActionPriority(E_Action[] eActions)
    {
        float totalProbability = 0;
        for (int i = 0; i < e_Actions.Length; i++)
        {
            float multiplier = lastActionIndex == i ? repeatPriorityModifier : 1;
            totalProbability += eActions[i].priority * multiplier; 
        }

        return totalProbability;
    }

    public int GetNextActionIndex()
    {
        float randomValue = UnityEngine.Random.Range(0.0f, GetTotalActionPriority(e_Actions));
        float cumulativeProbability = 0;

        for (int i = 0; i < e_Actions.Length; i++)
        {
            var e_Action = e_Actions[i];
            float multiplier = lastActionIndex == i ? repeatPriorityModifier : 1;
            cumulativeProbability += e_Action.priority * multiplier;
            if (randomValue <= cumulativeProbability)
            {
                cumulativeProbability -= e_Action.priority;
                lastActionIndex = i;
                return  i;
            }
        }
        return 0;
    }

}
