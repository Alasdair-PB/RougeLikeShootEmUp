using System;
using System.Collections;
using UnityEngine;
using Collectibles;

namespace Player
{

    [RequireComponent(typeof(P_Controller))]
    public class P_Collectibles : MonoBehaviour
    {

        [SerializeField] bool autoCollect = false; // Collect item when walking through
        private P_Actions my_playerActions;
        private static string collectibleTag = "Collectible";
        private Coroutine collectionCoroutine;


        private Action OnCollect;
        private bool collectOpen = false;
        private int collectibleCount;

        private void Awake()
        {
            my_playerActions = GetComponent<P_Actions>();
        }

        private void OnEnable()
        {
            my_playerActions.OnCollisionEnter += OnCollectibleCollision;
            my_playerActions.TryCollect += BeginActiveCollection;
            my_playerActions.TryDrop += DropCollectible; 
            OnCollect += OnCollectible;
        }

        private void OnDisable()
        {
            my_playerActions.OnCollisionEnter -= OnCollectibleCollision;
            my_playerActions.TryCollect -= BeginActiveCollection;
            my_playerActions.TryDrop -= DropCollectible;

            OnCollect -= OnCollectible;
        }

        // Using a coroutine to give the player time for an item to be collected if they are running past
        //  and not exactly on the object when they try to collect
        private void ClearCollectingStatus()
        {
            if (collectionCoroutine != null)
            {
                StopCoroutine(collectionCoroutine);
                collectionCoroutine = null;
            }
            collectOpen = false;
        }

        private void BeginActiveCollection()
        {
            ClearCollectingStatus();
            collectionCoroutine = StartCoroutine(CollectionWindow());
        }

        IEnumerator CollectionWindow()
        {
            collectOpen = true;
            yield return new WaitForSeconds(0.2f);
            collectOpen = false;
        }

        public void DropCollectible()
        {
            if (collectibleCount <= 0)
                return;

            var newCollectible = CollectibleSpawner.Instance.PullCollectible();
            newCollectible.transform.position = this.transform.position + (transform.forward * 3f);
            collectibleCount--;

            my_playerActions.SuccessfulDropEvent?.Invoke();
        }

        private void OnCollectible()
        {
            collectibleCount++;
        }

        private void OnCollectibleCollision(Collider other)
        {
            if (collectOpen == false && !autoCollect)
                return;

            if (other.CompareTag(collectibleTag))
            {
                OnCollect?.Invoke();
                ClearCollectingStatus();
                Collectible collectibleComponent = other.GetComponent<Collectible>();
                if (collectibleComponent != null)
                    collectibleComponent.OnCollect?.Invoke();
                else
                    throw new Exception("Item with Collectible tag does not contain component 'Collectible'");
            }
        }
    }

}