using System;
using UnityEngine;

namespace Enemies
{
    public class E_Actions : MonoBehaviour
    {
        public Action<E_Controller> OnDeath;
        public Action<Collider> OnCollision;
        public Action<int> OnDamage;
        public Action Reset;
    }
}
