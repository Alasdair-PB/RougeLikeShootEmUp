using Player;
using System;
using Unity.Mathematics;
using UnityEngine;

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

        private void Awake()
        {
            actions = GetComponent<P_Actions>();
            controller = GetComponent<P_Controller>();
            actions.A_LaunchProjectile += LaunchProjectile;

            if (projectilePool == null)
                projectilePool = FindAnyObjectByType<GlobalPooling>();
        }

        private void Start()
        {
            projectileMask = controller.GetC_Mask();
        }

        private void LaunchProjectile()
        {
            var weapon = weapons[weaponIndex];
            for (int i = 0; i < weapon.formations.Length; i++) {

                var playerCurrentRotation = controller.GetProjectileDirctionFloat2();
                float degrees = weapon.formations[i].angle;
                float radians = degrees * Mathf.Deg2Rad;

                float cos = Mathf.Cos(radians);
                float sin = Mathf.Sin(radians);

                float2 direction = new float2(
                    playerCurrentRotation.x * cos - playerCurrentRotation.y * sin,
                    playerCurrentRotation.x * sin + playerCurrentRotation.y * cos
                );

                var position = controller.GetPosition() + weapon.formations[i].positionOffset;
                var objectInPool = projectilePool.GetProjectilePool(weapon.projectilePrefab, 10, 999);
                objectInPool.InstantiateProjectile(direction, projectileMask, position);
            }
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
