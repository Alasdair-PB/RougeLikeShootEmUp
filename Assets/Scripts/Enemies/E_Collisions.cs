using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(E_Controller), typeof(E_Actions))]
    public class E_Collisions : MonoBehaviour
    {
        private E_Controller e_Controller;
        private E_Actions eActions;
        [SerializeField] private LayerMask damageMask;

        private void Awake()
        {
            eActions = GetComponent<E_Actions>();
            e_Controller = GetComponent<E_Controller>();
        }

        private void OnEnable() => eActions.OnCollision += OnCollision;
        private void OnDisable() => eActions.OnCollision -= OnCollision;
        private void OnTriggerEnter(Collider other) => eActions.OnCollision?.Invoke(other);
        private void OnTriggerStay(Collider other) => eActions.OnCollision?.Invoke(other);

        public void OnCollision(Collider other)
        {
            if (other.transform.root.TryGetComponent<C_OnContact>(out var onContact))
            {
                if (onContact.GetLayerMask() == damageMask)
                {
                    var colType = onContact.OnContact();

                    for (int i = 0; i < colType.Length; i++) {
                        switch (colType[i].c_type)
                        {
                            case (C_Types.Damage):
                                eActions.OnDamage?.Invoke(colType[i].damage);
                                break;
                            case C_Types.Death:
                                eActions.OnDeath?.Invoke(e_Controller); 
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}