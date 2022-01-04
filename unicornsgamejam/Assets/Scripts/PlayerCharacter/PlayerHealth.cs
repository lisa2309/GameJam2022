using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Update(){
        Debug.Log(currentHealth);
    }

    public void LooseHealth(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //FindObjectOfType<LevelLoader>().RestartLevel();
        }
        }
    }
}