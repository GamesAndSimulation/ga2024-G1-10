namespace props_positioning
{
    using UnityEngine;

    public class CoinCollision : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    Destroy(gameObject); // Destroy the coin object
                    playerStats.CollectCoin();
                    Debug.Log("coin picked up!");
                }
            }
        }
    }
}