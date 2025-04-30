using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music_Cs : MonoBehaviour
{
    private static Music_Cs instance;
    
    private AudioSource audioSource;
    
    private int currentSceneIndex = -1;
    
    [SerializeField] private AudioClip zone1Music;
    [SerializeField] private AudioClip zone2Music;
    [SerializeField] private AudioClip zone3Music;
    [SerializeField] private AudioClip menuMusic;
    
    public float volume = 1.0f;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.volume = volume;
        if (currentSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            CheckSongTrack();
        }
    }

    void CheckSongTrack()
    {
        if (currentSceneIndex is < 6 and > 0 && audioSource.resource != zone1Music)
        {
            audioSource.resource = zone1Music;
            audioSource.Play();
            return;
        }
        if (currentSceneIndex is < 11 and > 5 && audioSource.resource != zone2Music)
        {
            audioSource.resource = zone2Music;
            audioSource.Play();
            return;
        }
        if (currentSceneIndex is < 16 and > 10 && audioSource.resource != zone3Music)
        {
            audioSource.resource = zone3Music;
            audioSource.Play();
            return;
        }
        if (currentSceneIndex == 0)
        {
            audioSource.resource = menuMusic;
            audioSource.Play();
        }
    }
}
