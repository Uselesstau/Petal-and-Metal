using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SFX_Cs : MonoBehaviour
{
    private AudioSource audioSource;
    public List<AudioClip> audioClips;
    private Music_Cs music;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        music = GameObject.Find("MusicPlayer").GetComponent<Music_Cs>();
    }
    
    public enum SoundType
    {
        PetalLanding, MetalLanding, Jumping, Spring, Death, Splash
    }

    public void PlaySound(SoundType soundType, float pitch, float pitchRange)
    {
        AudioClip clip = audioClips[(int)soundType];
        audioSource.volume = music.volume * 0.6f;
        audioSource.pitch = Random.Range(pitch - pitchRange, pitch + pitchRange);
        audioSource.PlayOneShot(clip);
    }
}
