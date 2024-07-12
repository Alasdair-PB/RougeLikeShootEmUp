using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    // Some very basic object pooling for collectibles
    public class CollectibleSpawner : MonoBehaviour
    {
        public static CollectibleSpawner Instance { get; private set; }
        [SerializeField] GameObject collectiblePrefab;
        [SerializeField] int collectibleCount;
        [SerializeField] private Vector2 boundariesX, boundariesY, boundariesZ;
        private Stack<GameObject> collectiblePool = new Stack<GameObject>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void PushCollectible(GameObject collectible)
        {
            collectible.SetActive(false);
            collectiblePool.Push(collectible);
        }

        public GameObject PullCollectible()
        {
            if (collectiblePool.Count > 0)
            {
                var col = collectiblePool.Pop();
                col.SetActive(true);
                return col;
            }
            else
                return Instantiate(collectiblePrefab);
        }

        private void SpawnCollectible()
        {
            float randomX = Random.Range(boundariesX.x, boundariesX.y);
            float randomY = Random.Range(boundariesY.x, boundariesY.y);

            var initialCol = PullCollectible();
            initialCol.transform.position = new Vector3(randomX, randomY, 0f);
        }


        private void Start()
        {
            for (int i = 0; i < collectibleCount; i++)
            {
                SpawnCollectible();
            }
        }
    }
}
