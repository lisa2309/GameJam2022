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

    private void Update()
    {
        //Check if Rooted Tile is around
        if (manager.isRootedTileAround(transform.position))
            killPlant();
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= fadeSpeed; f -= fadeSpeed)
        {
            Color c = renderer.color;
            c.a = f;
            renderer.color = c;
            Debug.Log(f);
            yield return new WaitForSeconds(fadeSpeed);
        }
        Destroy(gameObject);
    }

    public void killPlant()
    {
        //Debug.Log("I ran into you");
        StartCoroutine("FadeOut");
    }
}