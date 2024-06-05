using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.CollectCoin();
                Destroy(gameObject); // Destroy the coin after collection
            }
        }
    }
}