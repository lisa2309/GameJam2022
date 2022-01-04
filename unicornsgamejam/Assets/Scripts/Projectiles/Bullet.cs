using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //cached references
    private Rigidbody2D rb;

    //config
    [SerializeField]
    private float velocity = 10.0f;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private LayerMask stoppingLayers;
    [SerializeField]
    private LayerMask targetLayers;
    [SerializeField]
    private float fallingGravityScale = 2f;
    public GameObject player;

    [SerializeField]
    private GameObject mushroom;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity * transform.right;
        player = GameObject.Find("PlayerCharacter");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((stoppingLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("Bullet X: " + transform.position.x + "Bullet Y: " + transform.position.y + "Bullet Z: " + transform.position.z);
            //Instantiate(mushroom, player.transform.position, player.transform.rotation);
            player.transform.position = transform.position;
            Destroy(gameObject);
            player.GetComponent<PlayerHealth>().LooseHealth(damage);

        }
        /*else if ((targetLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            Enemy hitEnemy = collision.gameObject.GetComponent<Enemy>();
            if (hitEnemy != null)
            {
                Destroy(hitEnemy.gameObject);
                FindObjectOfType<LevelLoader>().DecrementEnemyCount();
            }
            else
            {
                collision.gameObject.GetComponent<PlayerHealth>().LooseHealth(damage);
            }
            Destroy(gameObject);
        }*/
    }
}