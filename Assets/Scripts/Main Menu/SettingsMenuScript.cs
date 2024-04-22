using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuScript : MonoBehaviour
{
    public AudioManagerScript audioManager;
    public AudioMixer myMixer;
    public Slider musicSlider;
    public Slider SFXSlider;
    private float previousSFXVolume;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponentInParent<AudioManagerScript>();

        previousSFXVolume = SFXSlider.value;
    }

    void Update()
    {
        Debug.Log(PlayerPrefs.GetInt("Level"));
        if(SFXSlider.value != previousSFXVolume && Input.GetMouseButtonUp(0) && SceneManager.GetSceneByName("MainMenu").isLoaded) 
        {
            audioManager.PlaySFX(audioManager.bowShoot);

            previousSFXVolume = SFXSlider.value;
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value; 
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume); 
    }


    public void LoadVolume()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
}
