using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class P_Actions : MonoBehaviour
    {
        public Action<Vector2> OnMove;
        public Action<bool> OnGrounded;
        public Action<Collider> OnCollision;
        public Action SuccessfulDropEvent, TryDrop, TryCollect, OnRespawn, OnQuickStep, OnDeath;
        public Action<bool> OnDashEvent;
        public Action A_LaunchProjectile;

    }
}
