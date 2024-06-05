using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerChaos : MonoBehaviour
{

    public float restartDelay = 2f;
    public GameObject failLevelUI;
    public AudioSource deathMusic;
    public AudioSource bossMusic;


    public void EndGame(bool gameHasEnded)
    {
        failLevelUI.SetActive(true);
        bossMusic.Stop();
        deathMusic.Play();
        Debug.Log("Game Over!");
        SceneManager.LoadScene(8);

    }
}

