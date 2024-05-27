using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public float restartDelay = 1f;
    public GameObject failLevelUI;


    public void EndGame(bool gameHasEnded)
    {
        failLevelUI.SetActive(true);
        Debug.Log("Game Over!");
        Invoke("Restart", restartDelay);
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
