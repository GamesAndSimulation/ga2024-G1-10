using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text currentHealthText;
    [SerializeField] private TMP_Text maxHealthText;
    [SerializeField] private Image HUDImage;
    private TMP_Text coinText;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        currentHealthText.text = currentHealth.ToString();
        maxHealthText.text = maxHealth.ToString();

        if (currentHealth <= 0)
        {
            HUDImage.color = new Color(1f, 0f, 0f, 120f / 255f);
        }
        else
        {
            HUDImage.color = new Color(1f, 0f, 0f, 0f);
        }
    }
    
    public void UpdateCoins(int currentCoins, int totalCoins)
    {
        coinText.text = "Coins: " + currentCoins + " / " + totalCoins;
    }
}
