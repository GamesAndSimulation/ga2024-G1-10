using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChaosOutro : MonoBehaviour
{
    public void RetryGame()
    {
        SceneManager.LoadScene(6);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
