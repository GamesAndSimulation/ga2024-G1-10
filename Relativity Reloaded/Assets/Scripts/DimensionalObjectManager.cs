using UnityEngine;
using System.Collections.Generic;

public class DimensionalObjectManager : MonoBehaviour
{
    public static DimensionalObjectManager Instance { get; private set; }
    private List<GameObject> dimensionalObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterDimensionalObject(GameObject obj)
    {
        if (!dimensionalObjects.Contains(obj))
        {
            dimensionalObjects.Add(obj);
        }
    }

    public void UnregisterDimensionalObject(GameObject obj)
    {
        if (dimensionalObjects.Contains(obj))
        {
            dimensionalObjects.Remove(obj);
        }
    }

    public List<GameObject> GetDimensionalObjects()
    {
        return new List<GameObject>(dimensionalObjects);
    }
}
