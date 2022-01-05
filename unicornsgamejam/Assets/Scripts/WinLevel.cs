using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevel : MonoBehaviour
{
    new private SpriteRenderer renderer;
    public Sprite door_open;
    public GameManager gameManager;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered");
        renderer.sprite = door_open;
        gameManager.levelWon();
    }

}
