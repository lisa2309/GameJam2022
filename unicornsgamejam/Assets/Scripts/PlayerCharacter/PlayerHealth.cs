using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //state
    private int currentHealth;
    public ProgressBar Pb;

    //config
    [SerializeField]
    public int maxHealth = 5;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Update(){
        Pb.BarValue = currentHealth;
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