namespace props_positioning
{
    using UnityEngine;

    public class GunManager : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.HasGun = true;
                    gameObject.SetActive(false); // Deactivate the gun object
                    Debug.Log("Gun picked up!");
                }
            }
        }
    }
}