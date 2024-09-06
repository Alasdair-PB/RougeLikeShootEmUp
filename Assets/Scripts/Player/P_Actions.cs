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
        public Action<Collider> OnCollisionEnter, OnCollisionExit, OnCollisionStay;
        public Action SuccessfulDropEvent, TryDrop, TryCollect, OnRespawn, OnQuickStep, OnDeath, SwitchWeapon;
        public Action<bool> OnDashEvent, OnTransformEvent;
        public Action A_LaunchProjectile;
    }
}
