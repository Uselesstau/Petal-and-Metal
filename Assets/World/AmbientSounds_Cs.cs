using System.Collections;
using System.Data;
using UnityEngine;

public class AmbientSounds_Cs : MonoBehaviour
{
    private float timer;
    private AudioSource audioSource;
    private Music_Cs music;

    private float maxVolume;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        music = GameObject.Find("MusicPlayer").GetComponent<Music_Cs>();
        maxVolume = music.volume * 0.8f;
        SetTimer();
    }

    void SetTimer()
    {
        timer = Random.Range(10f, 45f);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartCoroutine(AudioFadeIn());
            SetTimer();
        }
    }

    IEnumerator AudioFadeIn()
    {
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < maxVolume)
        {
            if (maxVolume == 0)
            {
                break;
            }
            audioSource.volume += Time.deltaTime * maxVolume * 0.5f;
            yield return null;
        }
        StartCoroutine(AudioFadeOut());
    }
    IEnumerator AudioFadeOut()
    {
        yield return new WaitForSeconds(0.2f);
        audioSource.volume = maxVolume;
        while (audioSource.volume > 0)
        {
            if (maxVolume == 0)
            {
                break;
            }
            audioSource.volume -= Time.deltaTime * maxVolume * 0.5f;
            yield return null;
        }
    }
}
