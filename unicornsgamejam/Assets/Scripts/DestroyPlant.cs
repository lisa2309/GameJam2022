using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlant : MonoBehaviour
{
    private SpriteRenderer renderer;
    public float fadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        killPlant();
    }

    public void killPlant()
    {
        //Debug.Log("I ran into you");
        StartCoroutine("FadeOut");
    }
}
