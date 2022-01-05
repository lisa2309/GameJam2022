using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameEnded = false;
    private float delay = 1.0f;
    public GameObject completeLvlUI;

    public void endGame ()
    {
        if (gameEnded == false)
        {
            gameEnded = true;
            Invoke("Restart", delay);
        }
    }

    public void levelWon ()
    {
        completeLvlUI.SetActive(true);
        Invoke("NextLevel", delay);
    }

    private void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
