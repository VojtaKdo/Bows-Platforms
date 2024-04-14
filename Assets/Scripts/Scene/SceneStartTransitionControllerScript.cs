using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneStartTransitionControllerScript : MonoBehaviour
{
    PlayerMovementScript playerMovement;
    public float fadeDuration = 1.0f; // Duration of the fade effect
    public float holdDuration = 1.0f; // Duration to hold the screen black
    public Color fadeColor = Color.black; // Color to fade to
    public Image transitionImage; // Reference to the Image component
    public Image backgroundImage; // Reference to the Image component

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
    }
    public IEnumerator StartTransition()
    {
        // Fade in
        playerMovement.canMove = false;
        transitionImage.enabled = true;
        yield return FadeImage(transitionImage, 0f, 0f);
        yield return FadeImage(transitionImage, 1f, fadeDuration / 2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        //Vezme to aktuální barvu obrázku
        Color currentColor = image.color;
        
        //Vypoèítá, jak moc se má mìnit barva alfy za sekundu
        float alphaChangePerSecond = Mathf.Abs(currentColor.a - targetAlpha) / duration;

        //Loop dokud nová hodnota alfy barvy obrázku nedojde na targetAlpha
        while (!Mathf.Approximately(image.color.a, targetAlpha))
        {
            //Vypoèítá novou hodnotu alfy na základì alphaChangePerSecond
            float newAlpha = Mathf.MoveTowards(image.color.a, targetAlpha, alphaChangePerSecond * Time.deltaTime);

            //Zmìní barvu obrázku s novou hodnotou alfy a nechá nezmìnìní ostatní barvy
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

            //Poèká na další snímek pøedtím než bude pokraèovat loop
            yield return null;
        }
    }
}
