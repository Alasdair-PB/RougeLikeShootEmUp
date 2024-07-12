using UnityEngine;


namespace Player
{
    [RequireComponent(typeof(CapsuleCollider), typeof(P_Controller))]
    public class P_Collision : MonoBehaviour
    {
        private P_Actions pActions;

        private void Awake()
        {
            pActions = GetComponent<P_Actions>();
        }

        private void OnTriggerEnter(Collider other) =>
            pActions.OnCollision?.Invoke(other);

        private void OnTriggerStay(Collider other) =>
            pActions.OnCollision?.Invoke(other);

    }
}
