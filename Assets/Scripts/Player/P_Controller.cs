using System;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;


namespace Player
{
    public class P_Controller : MonoBehaviour
    {
        [SerializeField] private LayerMask projectileMask;
        [SerializeField] private P_Properties pProps;
        [SerializeField] private Game game;

        [SerializeField] private float2 xBounds, yBounds;
        [SerializeField] private bool pointsToDirection;

        private P_Actions pActions;
        private bool isDashing, isTransformed;

        /* Seperated out so players recieve immediate feedback on turning- while it would be more realistic for 
        delayed turning with velocity changes- it isn't fun for the player*/
        private Vector3 externalVelocity, moveVelocity, startPos;
        private Vector2 direction;

        public float2 GetPosition() => new float2(transform.position.x, transform.position.y);
        public void OnDash(bool state) => isDashing = state;
        public void SetTransformed(bool state) => isTransformed = state;

        public P_Properties GetP_Props() => pProps;
        public LayerMask GetC_Mask() => projectileMask;

        //[SerializeField] public UnityEvent DropEvent; // Alternative if events want to be serialized


        private void Awake()
        {
            pActions = GetComponent<P_Actions>();
            game.StartGame += Reset;
            startPos = transform.position;
        }

        private void OnEnable()
        {
            pActions.OnQuickStep += QuickStep;
            pActions.OnMove += SetMoveDirection;
            pActions.OnRespawn += Respawn;
            pActions.OnDashEvent += OnDash;
            pActions.OnDeath += DestroySelf;
            pActions.OnTransformEvent += SetTransformed;
        }

        private void OnDisable()
        {
            pActions.OnQuickStep -= QuickStep;
            pActions.OnMove -= SetMoveDirection;
            pActions.OnRespawn -= Respawn;
            pActions.OnDashEvent -= OnDash;
            pActions.OnDeath -= DestroySelf;
            pActions.OnTransformEvent -= SetTransformed;
        }

        private void OnDestroy()
        {
            game.StartGame -= Reset;
        }

        private void Reset()
        {
            this.gameObject.SetActive(true);
            transform.position = startPos;
            pActions.OnTransformEvent?.Invoke(false);
            // Reset health
        }

        private void DestroySelf()
        {
            game.EndGame?.Invoke(false);
            gameObject.SetActive(false);
        }
        public Vector3 GetDirctionVector3() => new Vector3(direction.x, direction.y, 0);
        public float2 GetDirctionFloat2() => new float2(direction.x, direction.y);
        public float2 GetProjectileDirctionFloat2() => new float2(0, 1);
        private void SetMoveDirection(Vector2 direction) => this.direction = direction.normalized;
        private void Respawn() => transform.position = startPos;

        // Updates the walkVelocity by new player direction
        private void UpdateMoveVelocity()
        {
            float maxMagnitude = 0;
            float accModifier = 0;

            if (isTransformed)
            {
                maxMagnitude = isDashing ? pProps.dashMaxVelocity * pProps.transformDMModifier
                    : pProps.moveMaxVelocity * pProps.transformMMModifier;
                accModifier = isDashing ? pProps.dashSpeed * pProps.transformDFModifier  // Wrong player stat
                    : pProps.moveSpeed * pProps.transformMSModifier;
            }
            else
            {
                maxMagnitude = isDashing ? pProps.dashMaxVelocity : pProps.moveMaxVelocity;
                accModifier = isDashing ? pProps.dashSpeed : pProps.moveSpeed;
            }

            Vector3 acceleration = accModifier * GetDirctionVector3();
            Vector3 updatedVelocity = moveVelocity + acceleration;
            updatedVelocity *= pProps.moveFriction;

            updatedVelocity = Vector3.ClampMagnitude(updatedVelocity, maxMagnitude);

            moveVelocity = updatedVelocity;
        }

        // Added for scenarios where the player may be pushed back- e.g conveyor mechanics
        public void ApplyExternalForce(Vector3 direction, float strength)
        {
            externalVelocity += direction.normalized * strength;
        }

        private void QuickStep()
        {
            var qsForce = isTransformed ? pProps.quickStepForce * pProps.transformQSModifier : pProps.quickStepForce;
            ApplyExternalForce(direction, qsForce);
        }


        // Remove all vertical force when grounded otherwise slow momentum gradually and update position
        private void ApplyForces()
        {
            Vector3 totalVelocity = (moveVelocity + externalVelocity) * Time.deltaTime;
            var nextPos = ApplyBoundaries(totalVelocity + transform.position);
            transform.position = nextPos;
            externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, pProps.worldFriction);

        }

        private void UpdateRotation()
        {
            transform.LookAt(transform.position + GetDirctionVector3());
        }


        // Stops the player exiting a fixed boundary box
        private float3 ApplyBoundaries(float3 nextPos)
        {
            if (nextPos.x < xBounds.x)
                nextPos.x = xBounds.x;
            else if (nextPos.x > xBounds.y)
                nextPos.x = xBounds.y;

            if (nextPos.y < yBounds.x)
                nextPos.y = yBounds.x;
            else if (nextPos.y > yBounds.y)
                nextPos.y = yBounds.y;

            return nextPos;
        }

        // Updates on input
        private void Update()
        {
            if (pointsToDirection)
                UpdateRotation();
        }

        void FixedUpdate()
        {
            UpdateMoveVelocity();
            ApplyForces();
        }
    }
}
