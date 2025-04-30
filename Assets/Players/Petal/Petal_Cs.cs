using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Petal_Cs : MonoBehaviour
{
    private Movement_Cs movement;
    void Start()
    {
        movement = GetComponent<Movement_Cs>();
    }
    void ResetLevel()
    {
        if (movement.isDead) return;
        movement.isDead = true;
        StartCoroutine(movement.Death());
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
            ResetLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cloud"))
        {
            other.gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cloud"))
        {
            other.gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
