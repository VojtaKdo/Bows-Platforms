using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject EscapeMenu;
    public static bool GameIsPaused = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused)
            {
                EscapeMenu.SetActive(false);
                ResumeGame();
            }
            else {
                EscapeMenu.SetActive(true);
                PauseGame();
            }
        }
    }

    /*public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

    public void MainMenu() {
        SceneManager.LoadScene(0);
        ResumeGame();
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
