using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace props_positioning
{
    public class SciFiPropSpawner : MonoBehaviour
    {
        public string propFolder = "Assets/3D Sprite Sci-Fi Props/SpriteProps_Prefabs"; // Folder to load props from
        public int numberOfProps = 100; // Number of props to place per terrain
        public Vector3 areaSize = new Vector3(50, 0, 50); // Area size for prop placement
        private GameObject[] propPrefabs; // Array of sci-fi prop prefabs
        private Terrain[] terrains; // Array of all terrains in the scene

        void Start()
        {
            // Load all prefabs from the specified folder and filter by name
            propPrefabs = LoadFilteredPrefabsAtPath(propFolder);

            // Retrieve all terrains in the scene
            terrains = Terrain.activeTerrains;

            // Check if any prefabs were loaded
            if (propPrefabs == null || propPrefabs.Length == 0)
            {
                Debug.LogError("No prefabs found in folder: " + propFolder);
                return;
            }

            // Check if any terrains were found
            if (terrains == null || terrains.Length == 0)
            {
                Debug.LogError("No terrains found in the scene.");
                return;
            }

            // Spawn the specified number of props for each terrain
            foreach (Terrain terrain in terrains)
            {
                for (int i = 0; i < numberOfProps; i++)
                {
                    SpawnProp(terrain);
                }
            }
        }

        GameObject[] LoadFilteredPrefabsAtPath(string path)
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { path });
            List<GameObject> filteredPrefabs = new List<GameObject>();

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab.name.Contains("Cube") || prefab.name.Contains("Barrel"))
                {
                    filteredPrefabs.Add(prefab);
                }
            }

            return filteredPrefabs.ToArray();
        }

        void SpawnProp(Terrain terrain)
        {
            bool validPosition = false;
            Vector3 randomPosition = Vector3.zero;
            Quaternion randomRotation = Quaternion.identity;
            GameObject propPrefab = null;

            // Try to find a valid position
            for (int attempt = 0; attempt < 10; attempt++)
            {
                // Get terrain boundaries
                Vector3 terrainPosition = terrain.transform.position;
                Vector3 terrainSize = terrain.terrainData.size;

                // Random position within the defined area on the terrain
                float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
                float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);
                float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

                randomPosition = new Vector3(randomX, 0, randomZ); // Set Y to 0 initially

                // Randomly select a prop prefab
                propPrefab = propPrefabs[Random.Range(0, propPrefabs.Length)];

                // Add 0.5 to the Y position for Cube props
                if (propPrefab.name.Contains("Cube"))
                {
                    randomPosition.y += 0.5f;
                }
                

                // Random rotation
                randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                // Check if the position is valid (on a flat part of the terrain)
                if (IsPositionValid(randomPosition, terrain))
                {
                    validPosition = true;
                    break;
                }
            }

            if (validPosition)
            {
                // Random scale
                float randomScale = Random.Range(0.8f, 1.2f);

                // Instantiate the prop at the random position with random rotation and scale
                GameObject prop = Instantiate(propPrefab, randomPosition, randomRotation);

                // Adjust position to ensure no collision with the terrain
                AdjustPositionToAvoidCollision(prop, terrain);
            }
            else
            {
                Debug.LogWarning("Could not find a valid position to place the prop.");
            }
        }

        bool IsPositionValid(Vector3 position, Terrain terrain)
        {
            // Raycast downwards to check the surface normal
            Ray ray = new Ray(new Vector3(position.x, position.y + 10, position.z), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the hit surface is part of the terrain
                if (hit.collider.gameObject == terrain.gameObject)
                {
                    // Check the angle of the surface normal
                    float angle = Vector3.Angle(hit.normal, Vector3.up);
                    if (angle < 10) // Adjust this value to set the maximum allowable slope angle
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        void AdjustPositionToAvoidCollision(GameObject prop, Terrain terrain)
        {
            Collider collider = prop.GetComponent<Collider>();
            if (collider != null)
            {
                Bounds bounds = collider.bounds;

                // Check if the prop's position is below the terrain height
                Vector3 minBound = bounds.min;
                Vector3 maxBound = bounds.max;
                Vector3 propPosition = prop.transform.position;

                float minY = terrain.SampleHeight(new Vector3(minBound.x, 0, minBound.z));
                float maxY = terrain.SampleHeight(new Vector3(maxBound.x, 0, maxBound.z));

                if (propPosition.y < minY || propPosition.y < maxY)
                {
                    propPosition.y = Mathf.Max(minY, maxY) + bounds.extents.y;
                    prop.transform.position = propPosition;
                }
            }
        }
    }
}
