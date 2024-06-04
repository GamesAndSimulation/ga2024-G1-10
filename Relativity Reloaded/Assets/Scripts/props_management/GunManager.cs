namespace props_positioning
{
    using UnityEngine;

    public class GunManager : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
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