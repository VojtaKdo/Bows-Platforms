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
        //Vezme to aktu�ln� barvu obr�zku
        Color currentColor = image.color;
        
        //Vypo��t�, jak moc se m� m�nit barva alfy za sekundu
        float alphaChangePerSecond = Mathf.Abs(currentColor.a - targetAlpha) / duration;

        //Loop dokud nov� hodnota alfy barvy obr�zku nedojde na targetAlpha
        while (!Mathf.Approximately(image.color.a, targetAlpha))
        {
            //Vypo��t� novou hodnotu alfy na z�klad� alphaChangePerSecond
            float newAlpha = Mathf.MoveTowards(image.color.a, targetAlpha, alphaChangePerSecond * Time.deltaTime);

            //Zm�n� barvu obr�zku s novou hodnotou alfy a nech� nezm�n�n� ostatn� barvy
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

            //Po�k� na dal�� sn�mek p�edt�m ne� bude pokra�ovat loop
            yield return null;
        }
    }
}
