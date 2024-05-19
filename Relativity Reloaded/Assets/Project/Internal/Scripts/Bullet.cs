using UnityEngine;

namespace Project.Internal.Scripts
{
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
            Destroy(gameObject);
        }
    }
}