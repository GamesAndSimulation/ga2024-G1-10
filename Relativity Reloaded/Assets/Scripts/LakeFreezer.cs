using UnityEngine;

public class LakeFreezer : MonoBehaviour
{
    public Material lakeMaterial; // Reference to the lake's material
    public Color frozenColor = Color.cyan; // Color to change the lake to when frozen
    private Color originalColor; // To store the original color of the lake
    private Vector4 originalFoamSettings; // To store the original foam settings
    private Vector4 originalFoamMovement; // To store the original foam movement settings
    private Vector4 originalFoamTiling; // To store the original foam tiling settings
    private Vector4 originalFoamIntensityCutoff; // To store the original foam intensity and cutoff settings
    private bool isFrozen = false; // Track whether the lake is frozen or not

    private LowPolyWater.LowPolyWater waterMovementScript; // Reference to the LowPolyWater script

    // Start is called before the first frame update
    void Start()
    {
        // Automatically find the LowPolyWater script in the same GameObject
        waterMovementScript = GetComponent<LowPolyWater.LowPolyWater>();

        if (lakeMaterial != null)
        {
            // Store the original properties
            originalColor = lakeMaterial.GetColor("_BaseColor");
            originalFoamSettings = lakeMaterial.GetVector("_Foam"); // Assuming foam settings are in a vector
            originalFoamMovement = lakeMaterial.GetVector("_BumpDirection"); // Assuming foam movement settings are in a vector
            originalFoamTiling = lakeMaterial.GetVector("_BumpTiling"); // Assuming foam tiling settings are in a vector
            originalFoamIntensityCutoff = lakeMaterial.GetVector("_Foam"); // Assuming foam intensity and cutoff are in a vector

            Debug.Log($"Original Foam Settings: {originalFoamSettings}");
            Debug.Log($"Original Foam Movement: {originalFoamMovement}");
            Debug.Log($"Original Foam Tiling: {originalFoamTiling}");
            Debug.Log($"Original Foam Intensity and Cutoff: {originalFoamIntensityCutoff}");
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
        if (lakeMaterial != null)
        {
            // Change the color to frozen color
            lakeMaterial.SetColor("_BaseColor", frozenColor);

            // Remove foam by setting its intensity to zero
            lakeMaterial.SetVector("_Foam", Vector4.zero);

            // Stop foam movement by setting foam movement to zero
            lakeMaterial.SetVector("_BumpDirection", Vector4.zero);

            // Stop foam tiling by setting it to zero
            lakeMaterial.SetVector("_BumpTiling", Vector4.zero);
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
        if (lakeMaterial != null)
        {
            // Restore the original color
            lakeMaterial.SetColor("_BaseColor", originalColor);

            // Restore the original foam settings
            lakeMaterial.SetVector("_Foam", originalFoamSettings);

            // Restore the original foam movement settings
            lakeMaterial.SetVector("_BumpDirection", originalFoamMovement);

            // Restore the original foam tiling settings
            lakeMaterial.SetVector("_BumpTiling", originalFoamTiling);

            // Restore the original foam intensity and cutoff settings
            lakeMaterial.SetVector("_Foam", originalFoamIntensityCutoff);
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
