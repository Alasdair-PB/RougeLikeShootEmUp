using Player;
using System;
using Unity.Mathematics;
using UnityEngine;
using static Player.P_ProjectileLauncher.PlayerWeapon;

namespace Player
{
    public class P_ProjectileLauncher : MonoBehaviour
    {
        [SerializeField] GlobalPooling projectilePool;
        [SerializeField] private P_Actions actions;
        [SerializeField] private P_Controller controller;
        [SerializeField] private PlayerWeapon[] weapons;

        private int weaponIndex = 0;
        private float timeSinceLastFired;
        private LayerMask projectileMask;
        private bool transformed; 

        private void SetTransformed(bool state) => transformed = state;


        private void Awake()
        {
            actions = GetComponent<P_Actions>();
            controller = GetComponent<P_Controller>();
            actions.A_LaunchProjectile += LaunchProjectile;
            actions.SwitchWeapon += IncrementIndex;

            if (projectilePool == null)
                projectilePool = FindAnyObjectByType<GlobalPooling>();
        }

        private void OnDestroy()
        {
            actions.A_LaunchProjectile -= LaunchProjectile;
            actions.SwitchWeapon -= IncrementIndex;

        }

        private void OnEnable()
        {
            actions.OnTransformEvent += SetTransformed;
            weaponIndex = 0;
        }


        private void OnDisable()
        {
            actions.OnTransformEvent -= SetTransformed;
        }

        private void Start()
        {
            projectileMask = controller.GetC_Mask();
        }

        private void LaunchProjectile()
        {
            var weapon = weapons[weaponIndex];
            SetLaunchProperties(weapon.formations, weapon.projectilePrefab);

            if (transformed)
                SetLaunchProperties(weapon.addedFormations, weapon.projectilePrefab);
        }

        private void SetLaunchProperties(Variation[] formations, GameObject projectilePrefab)
        {
            for (int i = 0; i < formations.Length; i++)
            {
                var playerCurrentRotation = controller.GetProjectileDirctionFloat2();
                float degrees = formations[i].angle;
                float radians = degrees * Mathf.Deg2Rad;

                float cos = Mathf.Cos(radians);
                float sin = Mathf.Sin(radians);

                float2 direction = new float2(
                    playerCurrentRotation.x * cos - playerCurrentRotation.y * sin,
                    playerCurrentRotation.x * sin + playerCurrentRotation.y * cos
                );

                var position = controller.GetPosition() + formations[i].positionOffset;
                var objectInPool = projectilePool.GetProjectilePool(projectilePrefab, 10, 999);
                objectInPool.InstantiateProjectile(direction, projectileMask, position);
            }
        }

        public void IncrementIndex()
        {
            weaponIndex++;
            if (weaponIndex >= weapons.Length)
                weaponIndex = 0;
        }

        void FixedUpdate()
        {
            if (weapons.Length <= 0)
                return;

            if (Time.time - timeSinceLastFired < weapons[weaponIndex].burstInterval)
                return;

            //LaunchProjectile();
            actions.A_LaunchProjectile?.Invoke();
            timeSinceLastFired = Time.time;
        }

        [Serializable]
        public struct PlayerWeapon
        {
            public float burstInterval;
            public Variation[] formations;
            public Variation[] addedFormations;
            public GameObject projectilePrefab;

            [Serializable]
            public struct Variation
            {
                public int angle;
                public float2 positionOffset;
            }
        }

    }
}
