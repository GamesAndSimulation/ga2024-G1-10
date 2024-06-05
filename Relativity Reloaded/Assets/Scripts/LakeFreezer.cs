using UnityEngine;

public class LakeFreezer : MonoBehaviour
{
    public Material originalMaterial; // Reference to the lake's original material
    public Material frozenMaterial; // Reference to the lake's frozen material
    private bool isFrozen = false; // Track whether the lake is frozen or not

    private LowPolyWater.LowPolyWater waterMovementScript; // Reference to the LowPolyWater script
    private MeshRenderer meshRenderer; // Reference to the MeshRenderer component
    private PlayerStats playerStats; // Reference to the PlayerStats script

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerStats != null && playerStats.HasAllCoins() && !isFrozen)
            {
                FreezeLake();
            }
            else if (isFrozen)
            {
                UnfreezeLake();
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

        isFrozen = true;
        Debug.Log("Lake frozen");
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

        isFrozen = false;
        Debug.Log("Lake unfrozen");
    }
}
