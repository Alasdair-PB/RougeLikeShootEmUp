using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(P_Controller))]
    public class P_Collision : MonoBehaviour
    {
        private P_Actions pActions;
        [SerializeField] private LayerMask damageMask;

        private void Awake()
        {
            pActions = GetComponent<P_Actions>();
        }        
        
        private void OnEnable()
        {
            pActions.OnCollision += OnCollision;
        }

        private void OnDisable()
        {
            pActions.OnCollision -= OnCollision;
        }


        private void OnTriggerEnter(Collider other)
            => pActions.OnCollision?.Invoke(other);
        

        private void OnTriggerStay(Collider other)
            => pActions.OnCollision?.Invoke(other);

        public void OnCollision(Collider other)
        {
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
