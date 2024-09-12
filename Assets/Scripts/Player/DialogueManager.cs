using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;

namespace Player
{
    [RequireComponent(typeof(Menu_Actions))]
    public class DialogueManager : MonoBehaviour
    {
        public Action OnPerformanceComplete;
        private Menu_Actions m_Actions;
        private int choiceIndex, optionsCount;
        private bool selectContinue = false;

        private float selectThreshold = 0.75f;

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

                while (selectContinue == false)
                    yield return null;

                RemoveChoiceBindings();
                selectContinue = false;
                StartCoroutine(PerformInteraction(options[choiceIndex].overrideInteraction));
            }
        }

        private void FinishPerformance(InteractionBase performer)
        {
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

    }
}
