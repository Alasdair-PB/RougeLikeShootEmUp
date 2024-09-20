using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;

namespace NPC
{
    [RequireComponent(typeof(Menu_Actions))]
    public class DialogueManager : MonoBehaviour
    {

        [SerializeField] Game game;
        public Action OnPerformanceComplete;
        private Menu_Actions m_Actions;
        private int choiceIndex, optionsCount;
        private bool selectContinue = false;

        private void Awake()
        {
            m_Actions = GetComponent<Menu_Actions>();
        }

        public void SetUpPerformance() => m_Actions.Select += Continue;
        public void DisablePerformance() => m_Actions.Select -= Continue;
        private void Continue() => selectContinue = true;
        private void MoveCursorUp() => choiceIndex = choiceIndex >= optionsCount ? 0 : choiceIndex + 1;
        private void MoveCursorDown() => choiceIndex = choiceIndex == 0 ? optionsCount : choiceIndex - 1;

        private void SetChoiceBindings(int possibleOptions)
        {
            optionsCount = possibleOptions;
            choiceIndex = 0;
            m_Actions.MoveCusorUp += MoveCursorUp;
            m_Actions.MoveCursorDown += MoveCursorDown;
        }

        private void RemoveChoiceBindings()
        {
            m_Actions.MoveCusorUp -= MoveCursorUp;
            m_Actions.MoveCursorDown -= MoveCursorDown;
        }

        public IEnumerator PerformInteraction(Interaction interaction)
        {
            var performer = interaction.interaction;
            Debug.Log(performer.dialogue);
             
            if (performer.options.Length == 0)
            {
                while (selectContinue == false)
                    yield return null;
                selectContinue = false;
                FinishPerformance(performer);
            }
            else
            {
                var options = performer.options;
                SetChoiceBindings(options.Length - 1);

                foreach (var option in options)
                {
                    Debug.Log(option.option);
                }

                var completed = false;

                while (!completed)
                {
                    while (selectContinue == false)
                        yield return null;

                    if (!(options[choiceIndex].unlockCondition == null) && 
                        !(options[choiceIndex].unlockCondition.GetCondition().IsUnlocked(game)))
                        selectContinue = false;
                    else 
                        completed = true;
                }

                RemoveChoiceBindings();
                selectContinue = false;

                TriggerPerformanceEvents(performer);
                StartCoroutine(PerformInteraction(options[choiceIndex].overrideInteraction));
            }
        }


        private void FinishPerformance(InteractionBase performer)
        {
            TriggerPerformanceEvents(performer);

            if (performer.nextInteraction == null)
            {
                DisablePerformance();
                OnPerformanceComplete?.Invoke();
                OnPerformanceComplete = null;

            }
            else
            {
                StartCoroutine(PerformInteraction(performer.nextInteraction));
            }
        }

        private void TriggerPerformanceEvents(InteractionBase performer)
        {
            if (game == null)
                return;

            foreach (var dialogueEvent in performer.onComplete)
            {
                if (dialogueEvent != null)
                    dialogueEvent.OnEventCalled(game);
            }

            foreach (var option in performer.options)
            {
                foreach (var dialogueEvent in option.actionEvent)
                {
                    if (dialogueEvent != null)
                        dialogueEvent.OnEventCalled(game);
                }
            }

        }

    }
}
