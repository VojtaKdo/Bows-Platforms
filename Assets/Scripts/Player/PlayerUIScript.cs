using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public static bool GameIsPaused = false;
    public static bool canOpenEscapeMenu; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canOpenEscapeMenu) {
            if (GameIsPaused)
            {
                Cursor.visible = false;
                PauseMenu.SetActive(false);
                SettingsMenu.SetActive(false);
                ResumeGame();
            }
            else {
                Cursor.visible = true;
                PauseMenu.SetActive(true);
                PauseGame();
            }
        }
    }

    /*public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

    public void MainMenu() {
        canOpenEscapeMenu = false;
        SceneManager.LoadScene(0);
        ResumeGame();
    }

    public void OpenSettingsMenu()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void Back(){
        SettingsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    void PauseGame() {
        AudioListener.pause = true;
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    void ResumeGame() {
        AudioListener.pause = false;
        GameIsPaused = false;
        Time.timeScale = 1f;
    }
}
