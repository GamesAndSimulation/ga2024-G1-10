using UnityEngine;

public class DimensionZone : MonoBehaviour
{
    public Bounds bounds;

    void Start()
    {
        // Initialize the bounds based on the Box Collider
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            bounds = boxCollider.bounds;
        }
        else
        {
            Debug.LogError("BoxCollider component not found on the Cube.");
        }
    }

    public bool IsPlayerInside(Vector3 playerPosition)
    {
        return bounds.Contains(playerPosition);
    }
}