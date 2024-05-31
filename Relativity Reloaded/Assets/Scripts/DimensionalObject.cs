using UnityEngine;

public class DimensionalObject : MonoBehaviour
{
    private void Start()
    {
        DimensionalObjectManager.Instance.RegisterDimensionalObject(gameObject);
    }

    private void OnDestroy()
    {
        if (DimensionalObjectManager.Instance != null)
        {
            DimensionalObjectManager.Instance.UnregisterDimensionalObject(gameObject);
        }
    }
}
