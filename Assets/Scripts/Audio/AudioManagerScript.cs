using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip backgroundMusic;
    public AudioClip bossMusic;
    public AudioClip bowString;
    public AudioClip bowShoot;
    public AudioClip chargeShot;
    public AudioClip saskaScream;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip) { 
        SFXSource.PlayOneShot(sfxClip);
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public void PlayMusic(AudioClip musicClip) {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void StopMusic() {
        musicSource.Stop();
    }
}
