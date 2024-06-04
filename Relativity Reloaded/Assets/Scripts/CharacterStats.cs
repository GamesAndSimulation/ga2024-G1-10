using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected bool isDead;
    public GameManager gameManager;
    public AudioSource damageSound;


    public virtual void Alive()
    {
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            if (gameManager != null)
            {
                gameManager.EndGame(isDead);
            }
        }

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    private void SetHealthTo(int healthToSetTo)
    {
        health = healthToSetTo;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        damageSound.Play();
        Alive();
    }

    public void Heal(int heal)
    {
        health = maxHealth;
        Alive();
    }

    public void InitVariables()
    {
        maxHealth = 3;
        SetHealthTo(maxHealth);
        isDead = false;
    }
}
