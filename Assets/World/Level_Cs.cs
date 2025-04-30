using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Cs : MonoBehaviour
{
    private int playersAtExit;
    private FadeEffect fade;
    
    private GameObject petal;
    private GameObject metal;

    void Start()
    {
        petal = GameObject.Find("Petal");
        metal = GameObject.Find("Metal");
        fade = GameObject.Find("FadeEffect").GetComponent<FadeEffect>();
    }
    IEnumerator LevelComplete()
    {
        StartCoroutine(fade.FadeOut());
        yield return new WaitForSeconds(1/0.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playersAtExit++;
            
            if (playersAtExit == 2)
            {
                petal.GetComponent<Movement_Cs>().startingLevel = true;
                metal.GetComponent<Movement_Cs>().startingLevel = true;
                StartCoroutine(LevelComplete());
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playersAtExit--;
        }
    }
}
