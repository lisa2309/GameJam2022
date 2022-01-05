using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlant : MonoBehaviour
{
    new private SpriteRenderer renderer;
    private TileMapManager manager;
    public float fadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer> ();
        manager = GameObject.Find("TileMapManager").GetComponent<TileMapManager>();
    }

    private void FixedUpdate()
    {
        //Check if Rooted Tile is around
        if (manager.isRootedTileAround(transform.GetChild(0).transform.position))
            killPlant();
    }

    IEnumerator fadeOut()
    {
        Color c = renderer.color;
        while (c.r < 1 || c.b < 1)
        {
            c.r += 0.15f;
            c.b += 0.15f;
            renderer.color = c;
            yield return new WaitForSeconds(0.5f);
        }

        for (float f = 1f; f >= fadeSpeed; f -= fadeSpeed)
        {           
            c = renderer.color;
            c.a = f;
            renderer.color = c;
            yield return new WaitForSeconds(fadeSpeed);
        }
        Destroy(gameObject);
    }

    public void killPlant()
    {
        //Debug.Log("I ran into you");
        StartCoroutine("fadeOut");
    }
}