using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Handle bullet collision here
        Destroy(gameObject);
    }
}