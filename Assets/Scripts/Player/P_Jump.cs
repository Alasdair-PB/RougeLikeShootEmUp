using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player { 
    public class P_Jump : MonoBehaviour
    {
        private P_Actions my_PlayerActions;
        private P_Controller my_PlayerController;
        private P_Properties pProps;
        private LayerMask collisionMask;


        private Vector3 upVector = Vector3.up;



        private bool isGrounded, isJumping;

        private void GetP_Props() => pProps = my_PlayerController.GetP_Props();
        private void GetC_Mask() => collisionMask = my_PlayerController.GetC_Mask();

        private void Awake()
        {
            my_PlayerActions = GetComponent<P_Actions>();
            my_PlayerController = GetComponent<P_Controller>();
            GetP_Props();
            GetC_Mask();
        }

        private void OnEnable()
        {
            my_PlayerActions.OnGrounded += SetGrounded;
        }

        private void OnDisable()
        {
            my_PlayerActions.OnGrounded -= SetGrounded;
        }


        private void CheckGrounded()
        {
            Vector3 rayCastOrigin = transform.position;
            rayCastOrigin.y -= (pProps.characterHeight / 2) - pProps.groundCheckDistance;

            bool newIsGrounded;

            if (Physics.Raycast(rayCastOrigin, Vector3.down, pProps.groundCheckDistance, collisionMask))
                newIsGrounded = true;
            else
                newIsGrounded = false;

            if (isGrounded != newIsGrounded)
                my_PlayerActions.OnGrounded?.Invoke(newIsGrounded);
        }
        private void Jump()
        {
            if (!isGrounded || isJumping) return;

            my_PlayerController.ApplyExternalForce(upVector, pProps.jumpForce);
            isJumping = true;
        }

        private void ApplyGravity()
        {
            if (isGrounded) return;
            my_PlayerController.ApplyExternalForce(-upVector, pProps.gravityForce);
        }

        private void SetGrounded(bool state)
        {
            isGrounded = state;

            if (isGrounded)
                isJumping = false;
        }


        void FixedUpdate()
        {
            CheckGrounded();
            ApplyGravity();
        }
    }
}
