using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{
    //Start hry
    public static int Level;
    public static void GameStart()
    {
        PlayerTutorialScript.tutorialProgress = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + Level + 1);
    }
}
