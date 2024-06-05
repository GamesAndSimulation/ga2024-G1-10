using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ChaosOutro : MonoBehaviour
{
    [SerializeField] protected int score;
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        scoreText.text = score.ToString();
        SetupCursor();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(6);
        SetupCursor();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
        SetupCursor();
    }

    private void SetupCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Ensure an Event System is present
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }
}
