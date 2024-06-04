using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public float restartDelay = 7f;
    public GameObject failLevelUI;
    public AudioSource deathMusic;
    public AudioSource bossMusic;


    public void EndGame(bool gameHasEnded)
    {
        failLevelUI.SetActive(true);
        bossMusic.Stop();
        deathMusic.Play();
        Debug.Log("Game Over!");
        Invoke("Restart", restartDelay);
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
