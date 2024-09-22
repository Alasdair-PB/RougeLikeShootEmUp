using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;


namespace Player
{
    [RequireComponent(typeof(P_Actions), typeof(DialogueManager), typeof(P_Controller))]
    public class P_Interact : MonoBehaviour
    {

        [SerializeField] private float interactTimeOut;

        private DialogueManager dialogueManager;
        private P_Actions pActions;
        private Coroutine interactCoroutine;
        private P_InputManager in_manager;

        private bool interacting = false;
        private static string talkable = "Talkable";

        void Awake()
        {
            pActions = GetComponent<P_Actions>();
            in_manager = GetComponent<P_InputManager>();
            dialogueManager = GetComponent<DialogueManager>();
        }

        private void OnEnable()
        {
            pActions.OnCollisionStay += OnInteractionStay;
            pActions.SwitchWeapon += TryInteract;
        }

        private void OnDisable()
        {
            pActions.OnCollisionStay -= OnInteractionStay;
            pActions.SwitchWeapon -= TryInteract;
        }

        private void TryInteract()
        {
            if (interactCoroutine != null)
            {
                StopCoroutine(interactCoroutine);
                interactCoroutine = null;
            }
            interacting = true;
            interactCoroutine = StartCoroutine(TryForInteract());
        }

        private IEnumerator TryForInteract()
        {
            yield return new WaitForSeconds(interactTimeOut);
            interacting = false;
        }

        private void OnInteractionStay(Collider other)
        {
            if (!other.CompareTag(talkable))
                return;

            if (interacting)
            {
                StopCoroutine(interactCoroutine);
                interacting = false;
                var interactor = other.GetComponent<InteractableDialogue>();

                if (interactor == null || !interactor.IsInteractable())
                    return;

                var interaction = interactor.GetInteraction();

                if (interaction == null)
                    return;

                PausePlayer();
                OnDisable();
                dialogueManager.SetUpPerformance();
                dialogueManager.OnPerformanceComplete += interactor.OnInteractionComplete;
                dialogueManager.OnPerformanceComplete += ResumePlayer;
                dialogueManager.OnPerformanceComplete += OnEnable;
                StartCoroutine(dialogueManager.PerformInteraction(interaction));
            }
        }

        private void PausePlayer() => in_manager.SwitchBindings(BindMode.UI);
        private void ResumePlayer() => in_manager.SwitchBindings(BindMode.Ship);
        
    }
}
