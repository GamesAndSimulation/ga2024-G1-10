using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManagerChaos : MonoBehaviour
{
    public float restartDelay = 2f;
    public GameObject failLevelUI;
    public AudioSource deathMusic;
    public AudioSource bossMusic;

    public static int score;

    private void Start()
    {
        score = 0;
    }

    public void ManDied()
    {
        score = score + 1;
    }

    public int ReturnScore()
    {
        return score;
    }

    public void EndGame(bool gameHasEnded)
    {
        failLevelUI.SetActive(true);
        bossMusic.Stop();
        deathMusic.Play();
        Debug.Log("Game Over!");
        SceneManager.LoadScene(8);
    }
}