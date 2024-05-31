using UnityEngine;

public class DimensionalObject : MonoBehaviour
{
    public bool initialState = true; // Set this in the inspector to define the initial state

    private void Start()
    {
        DimensionalObjectManager.Instance.RegisterDimensionalObject(gameObject);
        SetInitialState();
    }

    private void OnDestroy()
    {
        if (DimensionalObjectManager.Instance != null)
        {
            DimensionalObjectManager.Instance.UnregisterDimensionalObject(gameObject);
        }
    }

    private void SetInitialState()
    {
        gameObject.SetActive(initialState);
    }
}
