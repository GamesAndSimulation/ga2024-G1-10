using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DimensionalObjectManager : MonoBehaviour
{
    public static DimensionalObjectManager Instance { get; private set; }
    private List<GameObject> dimensionalObjects = new List<GameObject>();
    public float fadeDuration = 1.0f;

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
        Debug.Log("Trying to register object: " + obj.name);
        if (!dimensionalObjects.Contains(obj))
        {
            dimensionalObjects.Add(obj);
        }
        else
        {
            Debug.LogWarning("Object already registered: " + obj.name);
        }
    }

    public void UnregisterDimensionalObject(GameObject obj)
    {
        if (dimensionalObjects.Contains(obj))
        {
            dimensionalObjects.Remove(obj);
        }
    }

    public void ToggleDimensionalObjects()
    {
        foreach (GameObject dimensionalObject in dimensionalObjects)
        {
            if (dimensionalObject != null)
            {
                Debug.Log("name of the dimensional object: " + dimensionalObject.name);
                if (dimensionalObject.activeSelf)
                {
                    StartCoroutine(FadeAndToggle(dimensionalObject, fadeDuration, false));
                }
                else
                {
                    StartCoroutine(FadeAndToggle(dimensionalObject, fadeDuration, true));
                }
            }
        }
    }

    private IEnumerator FadeAndToggle(GameObject dimensionalObject, float duration, bool fadeIn)
    {
        Renderer renderer = dimensionalObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            yield break;
        }

        Material material = renderer.material;
        Color initialColor = material.color;
        float elapsedTime = 0f;

        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        if (fadeIn)
        {
            dimensionalObject.SetActive(true);
        }

        // Set material to use fade mode
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        if (!fadeIn)
        {
            dimensionalObject.SetActive(false);
        }
    }
}
