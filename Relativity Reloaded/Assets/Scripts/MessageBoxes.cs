using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxes : MonoBehaviour
{
    public GameObject uiImage; // Reference to the UI Image GameObject
    public string playerTag = "Player"; // Tag used to identify the player
    public AudioSource notificationMusic;


    void Start()
    {
        if (uiImage != null)
        {
            uiImage.SetActive(false); // Ensure the UI Image is initially hidden
        }
        else
        {
            Debug.LogError("UI Image reference not set in the inspector");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (uiImage != null)
            {
                uiImage.SetActive(true); // Show the UI Image when the player enters the platform
                notificationMusic.Play();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (uiImage != null)
            {
                uiImage.SetActive(false); // Hide the UI Image when the player exits the platform
            }
        }
    }
}
