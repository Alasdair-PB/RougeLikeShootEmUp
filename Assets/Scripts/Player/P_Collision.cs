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
            if (other.transform.root.TryGetComponent<C_OnContact>(out var proControlller))
            {
                if (proControlller.GetLayerMask() == damageMask)
                    pActions.OnDeath?.Invoke();
            }
        }

    }
}
