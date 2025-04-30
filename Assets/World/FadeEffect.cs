using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private static FadeEffect instance;
    void Start()
    {
        DontDestroyOnLoad(this);
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    IEnumerator FadeIn(float speed = 0.8f)
    {
        yield return new WaitForSeconds(0.2f);
        
        float a = 1.0f;
        while (a > 0)
        {
            a -= speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
    }
    
    public IEnumerator FadeOut(float speed = 0.8f)
    {
        float a = 0f;
        while (a < 1.0f)
        {
            a += speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, a);
            yield return null;
        }
        
        StartCoroutine(FadeIn());
    }
}
