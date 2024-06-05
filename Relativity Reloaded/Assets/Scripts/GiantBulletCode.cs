using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBulletCode : MonoBehaviour
{
    public float lifetime = 5f; // Lifetime of the bullet in seconds

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var pStats = collision.gameObject.GetComponent<PlayerStats>();
            if (pStats != null)
            {
                pStats.TakeDamage(3);
            }
        }

        Destroy(gameObject);
    }
}
