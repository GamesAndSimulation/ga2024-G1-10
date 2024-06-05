using dimensions;
using UnityEngine;

public class LakeFreezer : MonoBehaviour
{
    public Material originalMaterial; // Reference to the lake's original material
    public Material frozenMaterial; // Reference to the lake's frozen material
    public GameObject waterInvisibleWall; // Reference to the waterInvisibleWall GameObject
    private bool isFrozen = false; // Track whether the lake is frozen or not

    private LowPolyWater.LowPolyWater waterMovementScript; // Reference to the LowPolyWater script
    private MeshRenderer meshRenderer; // Reference to the MeshRenderer component
    private PlayerStats playerStats; // Reference to the PlayerStats script
    private DimensionZone dimensionZone; // Reference to the DimensionZone script

    // Start is called before the first frame update
    void Start()
    {
        // Automatically find the LowPolyWater script in the same GameObject
        waterMovementScript = GetComponent<LowPolyWater.LowPolyWater>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Find the PlayerStats component in the player GameObject
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        // Find the DimensionZone component
        if (waterInvisibleWall != null)
        {
            Debug.Log("Found waterInvisibleWall");
            dimensionZone = waterInvisibleWall.GetComponent<DimensionZone>();
        }

        if (originalMaterial == null || frozenMaterial == null)
        {
            Debug.LogError("Materials not assigned.");
            return;
        }

        if (waterMovementScript == null)
        {
            Debug.LogError("LowPolyWater script not found on the GameObject.");
        }

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found in the player GameObject.");
        }

        if (dimensionZone == null)
        {
            Debug.LogError("DimensionZone component not found on the cube.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerStats != null && playerStats.hasFreezePower && !isFrozen)
            {
                Debug.Log("Freezing the lake...");
                FreezeLake();
            }
            else if (isFrozen && (dimensionZone == null || !dimensionZone.IsPlayerInside(playerStats.transform.position)))
            {
                UnfreezeLake();
            }
            else if (isFrozen)
            {
                Debug.Log("Cannot unfreeze the lake while inside the cube boundaries.");
            }
            else if (!playerStats.hasFreezePower)
            {
                Debug.Log("Cannot freeze the lake. Collect all coins first.");
            }
        }
    }

    // Method to freeze the lake
    public void FreezeLake()
    {
        if (meshRenderer != null)
        {
            // Switch to frozen material
            meshRenderer.material = frozenMaterial;
        }

        // Disable the water movement script if it exists
        if (waterMovementScript != null)
        {
            waterMovementScript.enabled = false;
        }

        // Deactivate the waterInvisibleWall
        if (waterInvisibleWall != null)
        {
            waterInvisibleWall.SetActive(false);
        }

        isFrozen = true;
        Debug.Log("Lake frozen and waterInvisibleWall deactivated");
    }

    // Method to unfreeze the lake
    public void UnfreezeLake()
    {
        if (meshRenderer != null)
        {
            // Switch to original material
            meshRenderer.material = originalMaterial;
        }

        // Enable the water movement script if it exists
        if (waterMovementScript != null)
        {
            waterMovementScript.enabled = true;
        }

        // Activate the waterInvisibleWall
        if (waterInvisibleWall != null)
        {
            waterInvisibleWall.SetActive(true);
        }

        isFrozen = false;
        Debug.Log("Lake unfrozen and waterInvisibleWall activated");
    }
}
