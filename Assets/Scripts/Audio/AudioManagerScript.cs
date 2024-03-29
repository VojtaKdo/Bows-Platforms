using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip backgroudnMusic;
    public AudioClip bowString;
    public AudioClip bowShoot;
    public AudioClip chargeShot;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = backgroudnMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip) { 
        SFXSource.PlayOneShot(sfxClip);
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }
}
