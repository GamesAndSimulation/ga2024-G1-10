using UnityEngine;

public class ChaosLogic : MonoBehaviour
{
    // Public variable to assign the entity prefab
    public GameObject entityPrefab;

    // Public variable to assign the spawn location object
    public GameObject spawnLocationObject;

    // Spawn scale
    private Vector3 spawnScale = new Vector3(2f, 2f, 2f);

    // Timer for tracking the 20-second interval
    private float spawnInterval = 20f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {

        timer = spawnInterval; // Initialize timer
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // Reduce timer by the time elapsed since last frame

        if (timer <= 0f)
        {
            SpawnEntity();
            timer = spawnInterval; // Reset timer
        }
    }

    // Method to spawn the entity at the position of the specified spawn location object
    void SpawnEntity()
    {
        if (entityPrefab != null && spawnLocationObject != null)
        {
            Vector3 spawnPosition = spawnLocationObject.transform.position;
            Debug.Log($"Spawning entity at position: {spawnPosition}");

            GameObject newEntity = Instantiate(entityPrefab, spawnPosition, Quaternion.identity);
            newEntity.transform.localScale = spawnScale;

            Debug.Log($"Spawned entity {newEntity.name} at position: {newEntity.transform.position}");
        }
        else
        {
            if (entityPrefab == null)
            {
                Debug.LogWarning("Entity prefab is not assigned.");
            }

            if (spawnLocationObject == null)
            {
                Debug.LogWarning("Spawn location object is not assigned.");
            }
        }
    }
}