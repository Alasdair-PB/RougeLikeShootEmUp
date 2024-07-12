using UnityEngine;


// Here as an example of how additional effects could be easily added- VFX, animation et al code could all look quite similar

namespace Player
{
    [RequireComponent(typeof(P_Controller), typeof(AudioSource))]
    public class P_Audio : MonoBehaviour
    {
        private P_Actions pActions;
        private AudioSource audioSource;
        [SerializeField] private AudioClip jumpSound, dropSound;

        private void Awake()
        {
            pActions = GetComponent<P_Actions>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            pActions.OnQuickStep += PlayJumpSound;
            pActions.SuccessfulDropEvent += PlayDropSound;
            // + any other events that could use sounds
        }

        private void OnDisable()
        {
            pActions.OnQuickStep -= PlayJumpSound;
            pActions.SuccessfulDropEvent -= PlayDropSound;
        }

        private void PlayJumpSound() =>
            audioSource.PlayOneShot(jumpSound);

        public void PlayDropSound() =>
            audioSource.PlayOneShot(dropSound);

    }
}
