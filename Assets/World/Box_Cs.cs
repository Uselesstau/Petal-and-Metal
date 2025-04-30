using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Box_Cs : MonoBehaviour
{
    private AudioSource audioSource;
    private Music_Cs music;

    [SerializeField] private AudioClip ground;
    [SerializeField] private AudioClip water;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        music = GameObject.Find("MusicPlayer").GetComponent<Music_Cs>();
        audioSource.volume = music.volume;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 6)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.clip = ground;
            audioSource.Play();
        }
        if (other.gameObject.layer == 4)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.clip = water;
            audioSource.Play();
        }
    }
}
