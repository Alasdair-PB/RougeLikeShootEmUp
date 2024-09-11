using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies
{

    public class E_Bounds : MonoBehaviour
    {
        [SerializeField] private E_Actions e_Actions;
        [SerializeField] private E_Controller e_Controller;
        private static string bounds = "Bounds";

        void Awake()
        {
            if (e_Actions == null )
                e_Actions = GetComponent<E_Actions>();
            if (e_Controller == null )
                e_Controller = GetComponent<E_Controller>();

            if (e_Controller == null)
                Debug.LogError("E_Controller has not been set on enemyBounds");
            if (e_Actions == null)
                Debug.LogError("E_Actions has not been set on enemyBounds");

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(bounds))
                e_Actions.OnDeath?.Invoke(e_Controller);
        }

    }
}
