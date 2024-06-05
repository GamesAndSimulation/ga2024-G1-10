namespace Project.Internal.Scripts.Enemies.enemies_manager
{
    using UnityEngine;

    public class OrcManager : MonoBehaviour
    {
        public GameObject pickupPrefab; // Reference to the pickup prefab
        public Transform spawnPoint; // Reference to the spawn point for the pickup
        private FirstOrcGang[] orcScripts;

        void Start()
        {
            // Find objects with the script FirstOrcGang
            orcScripts = FindObjectsOfType<FirstOrcGang>();
            Debug.Log("Found " + orcScripts.Length + " orcs");
        }

        void Update()
        {
            if (AreAllOrcsDead())
            {
                Debug.Log("All orcs are dead!");
                SpawnPickup();
            }
        }

        private bool AreAllOrcsDead()
        {
            foreach (FirstOrcGang orc in orcScripts)
            {
                if (orc != null)
                {
                    EnemyStats enemyStats = orc.GetComponent<EnemyStats>();
                    if (enemyStats != null && enemyStats.health > 0)
                    {
                        return false; // If any orc is still alive, return false
                    }
                }
            }
            return true; // All orcs are dead or destroyed
        }

        private void SpawnPickup()
        {
            Instantiate(pickupPrefab, spawnPoint.position, spawnPoint.rotation);
            // Optionally, destroy this script or object if you only want to spawn once
            Destroy(this);
        }
    }
}