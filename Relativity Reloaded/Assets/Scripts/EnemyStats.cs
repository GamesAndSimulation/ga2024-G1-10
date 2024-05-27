using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private int health = 3;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Handle enemy death
            Destroy(gameObject);
        }
    }

