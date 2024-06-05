using UnityEngine;

namespace props_management
{
    public class DimensionalPotion: MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.hasDimensionSwitch = true;
                    Destroy(gameObject); // Destroy the potion object
                    Debug.Log("Gun picked up!");
                }
            }
        }

    }
}