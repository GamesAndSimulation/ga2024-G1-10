using UnityEngine;

public class LakeFreezer : MonoBehaviour
{
    public Material originalMaterial; // Reference to the lake's original material
    public Material frozenMaterial; // Reference to the lake's frozen material
    private bool isFrozen = false; // Track whether the lake is frozen or not

    private LowPolyWater.LowPolyWater waterMovementScript; // Reference to the LowPolyWater script
    private MeshRenderer meshRenderer; // Reference to the MeshRenderer component

    // Start is called before the first frame update
    void Start()
    {
        // Automatically find the LowPolyWater script in the same GameObject
        waterMovementScript = GetComponent<LowPolyWater.LowPolyWater>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (originalMaterial == null || frozenMaterial == null)
        {
            Debug.LogError("Materials not assigned.");
            return;
        }

        if (waterMovementScript == null)
        {
            Debug.LogError("LowPolyWater script not found on the GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFrozen)
            {
                UnfreezeLake();
            }
            else
            {
                FreezeLake();
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
