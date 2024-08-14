using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(E_Actions), typeof(E_Controller))]
    public class E_Health : MonoBehaviour
    {
        // May change to EnemyProperties file later
        [SerializeField] int health;
        [SerializeField] float iFrames;

        private int currentHealth;
        private E_Actions e_Actions;
        private E_Controller e_Controller;

        private float timeAtLastDamage;

        private void Awake()
        {
            e_Controller = GetComponent<E_Controller>();
            e_Actions = GetComponent<E_Actions>();
        }

        private void OnEnable()
        {
            currentHealth = health;
            e_Actions.OnDamage += TakeDamage;
        }

        private void OnDisable()
        {
            e_Actions.OnDamage -= TakeDamage;
        }

        private void TakeDamage(int damage)
        {
            if (Time.time - timeAtLastDamage > iFrames)
            {
                currentHealth -= damage;
                timeAtLastDamage = Time.time;

                if (currentHealth < 0)
                    e_Actions.OnDeath?.Invoke(e_Controller);
            }
        }
    }
}
