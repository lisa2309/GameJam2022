using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //state
    private int currentHealth;

    //config
    [SerializeField]
    private int maxHealth = 3;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void LooseHealth(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            {
            FindObjectOfType<LevelLoader>().RestartLevel();
        }
        }
    }
}