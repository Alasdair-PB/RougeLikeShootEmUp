using System;
using UnityEngine;



namespace Collectibles
{
    // Adds itself to object pool when collected
    public class Collectible : MonoBehaviour
    {
        public Action OnCollect;

        void OnEnable()
            => OnCollect += Collect;

        private void OnDisable()
            => OnCollect -= Collect;

        public void Collect()
            => CollectibleSpawner.Instance.PushCollectible(this.gameObject);

    }
}
