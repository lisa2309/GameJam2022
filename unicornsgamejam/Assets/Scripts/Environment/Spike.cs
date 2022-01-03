using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       collision.gameObject.GetComponent<PlayerHealth>().LooseHealth(3);
    }
}
