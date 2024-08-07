using Player;
using System;
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
            var direction = controller.GetProjectileDirctionFloat2();
            var position = controller.GetPosition();
            var objectInPool = projectilePool.GetProjectilePool(weapons[weaponIndex].projectilePrefab, 10, 999);
            objectInPool.InstantiateProjectile(direction, projectileMask, position);
        }

        void FixedUpdate()
        {
            if (weapons.Length <= 0)
                return;

            if (Time.time - timeSinceLastFired < weapons[weaponIndex].burstInterval)
                return;

            LaunchProjectile();
            //actions.A_LaunchProjectile?.Invoke();
            timeSinceLastFired = Time.time;
        }

        [Serializable]
        public struct PlayerWeapon
        {
            public float burstInterval;
            public GameObject projectilePrefab;
        }

    }
}
