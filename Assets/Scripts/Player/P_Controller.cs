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

        private P_Actions pActions;
        private bool isDashing;

        /* Seperated out so players recieve immediate feedback on turning- while it would be more realistic for 
        delayed turning with velocity changes- it isn't fun for the player*/
        private Vector3 externalVelocity, moveVelocity, startPos;
        private Vector2 direction;

        public float2 GetPosition() => new float2(transform.position.x, transform.position.y);
        public void OnDash(bool state) => isDashing = state;
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
        }

        private void OnDisable()
        {
            pActions.OnQuickStep -= QuickStep;
            pActions.OnMove -= SetMoveDirection;
            pActions.OnRespawn -= Respawn;
            pActions.OnDashEvent -= OnDash;
            pActions.OnDeath -= DestroySelf;
        }


        private void OnDestroy()
        {
            game.StartGame -= Reset;
        }

        private void Reset()
        {
            this.gameObject.SetActive(true);
            transform.position = startPos;
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


            Vector3 acceleration = (isDashing ? pProps.dashSpeed : pProps.moveSpeed) * GetDirctionVector3();

            Vector3 updatedVelocity = moveVelocity + acceleration;

            updatedVelocity *= pProps.moveFriction;
            updatedVelocity = Vector3.ClampMagnitude(updatedVelocity, (isDashing ? pProps.dashMaxVelocity : pProps.moveMaxVelocity));

            moveVelocity = updatedVelocity;
        }

        // Added for scenarios where the player may be pushed back- e.g conveyor mechanics
        public void ApplyExternalForce(Vector3 direction, float strength)
        {
            externalVelocity += direction.normalized * strength;
        }

        private void QuickStep()
        {
            ApplyExternalForce(direction, pProps.quickStepForce);
        }


        // Remove all vertical force when grounded otherwise slow momentum gradually and update position
        private void ApplyForces()
        {
            Vector3 totalVelocity = moveVelocity + externalVelocity;

            transform.position += totalVelocity * Time.deltaTime;
            externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, pProps.worldFriction);

        }

        private void UpdateRotation()
        {
            transform.LookAt(transform.position + GetDirctionVector3());
        }


        // Stops the player exiting a fixed boundary box
        private void ApplyBoundaries()
        {

        }

        // Updates on input
        private void Update()
        {
            UpdateRotation();
        }

        void FixedUpdate()
        {
            UpdateMoveVelocity();
            ApplyForces();
        }
    }
}
