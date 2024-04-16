using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //Start hry
    public static int Level;
    public static bool NewGameCreated = false;
    public GameObject MainMenu;
    public GameObject CreditsMenu;

    public static void NewGame()
    {
        NewGameCreated = false;
        PlayerTutorialScript.tutorialProgress = 0;
        PlayerStatsScript.playerHP = PlayerStatsScript.playerMaxHP;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level") + 1);
    }

    public static void LoadGame() {
        if (NewGameCreated)
        {
            float playerSavedHP = PlayerPrefs.GetFloat("playerHP");
            PlayerStatsScript.playerHP = playerSavedHP;
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level") + 1);
            Debug.Log(PlayerPrefs.GetInt("Level") + 1); 
        }
        else {
            Debug.Log("New game wasn't created!");
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

    public void Back() {
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }
}
