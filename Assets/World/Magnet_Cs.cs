using System;
using UnityEngine;

public class Magnet_Cs : MonoBehaviour
{
    [SerializeField] private float strength;

    private GameObject metal;

    public bool isOn = true;

    void Start()
    {
        metal = GameObject.Find("Metal");
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, metal.transform.position);

        if (dist < 4 && isOn)
        {
            Vector3 dir = transform.position - metal.transform.position;
            metal.GetComponent<Rigidbody2D>().AddForce(dir * (Mathf.Pow(strength, 2) * Time.deltaTime * (1/dist)), ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == metal)
        {
            metal.GetComponent<Movement_Cs>().magnetized = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == metal)
        {
            metal.GetComponent<Movement_Cs>().magnetized = false;
        }
    }
}
