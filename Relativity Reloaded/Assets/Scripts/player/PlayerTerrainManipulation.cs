namespace Project.Internal.Scripts.Enemies.player
{
    using UnityEngine;
    using System.Collections.Generic;

    public class PlayerTerrainManipulation : MonoBehaviour
    {
        public Terrain terrain;

        public float power = 0.2f; // Smaller power for weaker changes
        public float radius = 10.0f;
        public float minRadius = 10.0f; // Minimum radius for manipulation
        public float maxRadius = 15.0f; // Maximum radius for manipulation
        public KeyCode raiseKey = KeyCode.R; // Key to raise the terrain
        public KeyCode lowerKey = KeyCode.F; // Key to lower the terrain

        private readonly Dictionary<Vector2Int, float> _originalHeights = new Dictionary<Vector2Int, float>();

        void Update()
        {
            HandleTerrainManipulation();
        }

        private void HandleTerrainManipulation()
        {
            if (Input.GetKeyDown(raiseKey))
            {
                ManipulateTerrain(true);
            }

            if (Input.GetKeyDown(lowerKey))
            {
                ManipulateTerrain(false);
            }
        }

        private void ManipulateTerrain(bool raise)
        {
            // Perform a raycast from the camera's position to where the player is looking
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits the terrain
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == terrain.gameObject)
            {
                // Get the hit point in world coordinates
                Vector3 hitPoint = hit.point;

                // Calculate the distance from the player to the hit point
                float distanceFromPlayer = Vector3.Distance(transform.position, hitPoint);

                // Ensure the hit point is within the valid radius range
                if (distanceFromPlayer < minRadius || distanceFromPlayer > maxRadius)
                {
                    return;
                }

                // Get the position of the terrain in the world
                Vector3 terrainPosition = terrain.transform.position;

                // Get the TerrainData from the terrain
                TerrainData terrainData = terrain.terrainData;

                // Calculate the base position in the heightmap corresponding to the hit point
                int xBase = (int)((hitPoint.x - terrainPosition.x) / terrainData.size.x *
                                  terrainData.heightmapResolution);
                int zBase = (int)((hitPoint.z - terrainPosition.z) / terrainData.size.z *
                                  terrainData.heightmapResolution);

                // Calculate the radius in terms of the heightmap resolution
                int radiusInHeights = Mathf.RoundToInt(radius / terrainData.size.x * terrainData.heightmapResolution);

                // Get the current heights in the area around the hit point
                float[,] heights = terrainData.GetHeights(xBase - radiusInHeights / 2, zBase - radiusInHeights / 2,
                    radiusInHeights, radiusInHeights);

                // Loop through the heightmap points in the specified radius
                for (int x = 0; x < radiusInHeights; x++)
                {
                    for (int z = 0; z < radiusInHeights; z++)
                    {
                        // Calculate the distance from the center of the radius
                        float distance = Vector2.Distance(new Vector2(x, z),
                            new Vector2(radiusInHeights / 2, radiusInHeights / 2));

                        // If the point is within the radius, adjust the height
                        if (distance <= radiusInHeights / 2)
                        {
                            // Calculate the adjustment based on the distance and power
                            float adjustment = (radiusInHeights / 2 - distance) / (radiusInHeights / 2) * power *
                                               Time.deltaTime;

                            // Store the original height for future reversion
                            Vector2Int point = new Vector2Int(xBase - radiusInHeights / 2 + x,
                                zBase - radiusInHeights / 2 + z);
                            if (!_originalHeights.ContainsKey(point))
                            {
                                _originalHeights[point] = heights[x, z];
                            }

                            // Raise or lower the terrain based on the input
                            if (raise)
                            {
                                heights[x, z] += adjustment;
                            }
                            else
                            {
                                heights[x, z] -= adjustment;
                            }
                        }
                    }
                }

                // Set the new heights back to the terrain
                terrainData.SetHeights(xBase - radiusInHeights / 2, zBase - radiusInHeights / 2, heights);
            }
        }

        public void RevertTerrain()
        {
            // Get the TerrainData from the terrain
            TerrainData terrainData = terrain.terrainData;

            foreach (var entry in _originalHeights)
            {
                int x = entry.Key.x;
                int z = entry.Key.y;
                float[,] heights = terrainData.GetHeights(x, z, 1, 1);
                heights[0, 0] = entry.Value;
                terrainData.SetHeights(x, z, heights);
            }

            // Clear the stored original heights after reverting
            _originalHeights.Clear();
        }
    }
}