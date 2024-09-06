using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Explorer;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

namespace Player
{
    [RequireComponent(typeof(P_Controller))]
    public class P_Collision : MonoBehaviour
    {
        private P_Actions pActions;
        private static string groundTag = "Ground", projectileObject = "DamageObject";
        [SerializeField] private LayerMask damageMask;

        private void Awake()
        {
            pActions = GetComponent<P_Actions>();
        }        
        
        private void OnEnable()
        {
            pActions.OnCollisionEnter += OnCollisionProjectileCheck;
            pActions.OnCollisionExit += OnCollisionExitGroundCheck;
            pActions.OnCollisionEnter += OnCollisionEnterGroundCheck;
            pActions.OnCollisionStay += OnCollisionEnterGroundCheck;
        }

        private void OnDisable()
        {
            pActions.OnCollisionEnter -= OnCollisionProjectileCheck;
            pActions.OnCollisionExit -= OnCollisionExitGroundCheck;
            pActions.OnCollisionEnter -= OnCollisionEnterGroundCheck;
            pActions.OnCollisionStay -= OnCollisionEnterGroundCheck;

        }

        private void OnTriggerEnter(Collider other)
        {
            pActions.OnCollisionEnter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
            => pActions.OnCollisionStay?.Invoke(other);

        private void OnTriggerExit(Collider other)
        {
            pActions.OnCollisionExit?.Invoke(other);
        }

        private void GroundCheck (bool enter, Collider other)
        {

            if (other.CompareTag(groundTag))
            {
                pActions.OnTransformEvent?.Invoke(enter);
                return;
            }
        }

        private void OnCollisionExitGroundCheck(Collider other) => GroundCheck(false, other);
        private void OnCollisionEnterGroundCheck(Collider other) => GroundCheck(true, other);

        private void OnCollisionProjectileCheck(Collider other)
        {
            if (!other.CompareTag(projectileObject))
                return;

            Transform currentTransform = other.transform;

            while (currentTransform != null)
            {
                if (currentTransform.TryGetComponent<C_OnContact>(out var proController))
                {
                    if (proController.GetLayerMask() == damageMask)
                    {
                        pActions.OnDeath?.Invoke();
                    }
                    return;
                }
                currentTransform = currentTransform.parent;
            }
            Debug.LogWarning("C_OnContact component not found in the hierarchy.");
        }
    }
}
