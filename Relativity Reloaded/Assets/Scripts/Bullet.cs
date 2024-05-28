using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; // Lifetime of the bullet in seconds

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Handle bullet collision here
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemyStats = collision.gameObject.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(1);
            }
        }

        Destroy(gameObject);
    }
}
