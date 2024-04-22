using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //Start hry
    public SettingsMenuScript settingsMenu;
    public static int Level;
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject SettingsMenu;

    void Start()
    {
        settingsMenu.LoadVolume();
        Cursor.visible = true;
    }
    public static void NewGame()
    {
        Cursor.visible = false;
        PlayerTutorialScript.tutorialProgress = 0;
        PlayerStatsScript.playerHP = PlayerStatsScript.playerMaxHP;
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetInt("Level", 1);
        Level = PlayerPrefs.GetInt("Level");
        SceneManager.LoadScene(Level);
    }

    public static void LoadGame() {
        if (PlayerPrefs.GetInt("NewGameCreated") == 1)
        {
            Cursor.visible = false;
            float playerSavedHP = PlayerPrefs.GetFloat("playerHP");
            PlayerStatsScript.playerHP = playerSavedHP;
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        }
        else { 
            Debug.Log("New game isn't created!");
        }
    }

    public static void ExitGame()
    {
        Debug.Log("Player left the game!");
        Application.Quit();
    }

    public void OpenCreditsMenu() { 
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void OpenSettingsMenu() {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void SzadiArt() {
        Application.OpenURL("https://szadiart.itch.io/");
    }

    public void Anakolisa() {
        Application.OpenURL("https://anokolisa.itch.io/");
    }

    public void Kronovi() {
        Application.OpenURL("https://itch.io/profile/darkpixel-kronovi");
    }

    public void CraftPix() {
        Application.OpenURL("https://craftpix.net/freebies/free-skeleton-pixel-art-sprite-sheets/");
    }

    public void ChrisKohler()
    {
        Application.OpenURL("https://chriskohler.net/");
    }

    public void JohnLeonardFrench()
    {
        Application.OpenURL("https://johnleonardfrench.com/");
    }

    public void Back() {
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        SettingsMenu.SetActive(false);
    }
}
