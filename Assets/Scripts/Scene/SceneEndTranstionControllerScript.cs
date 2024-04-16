using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class SceneEndTranstionControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerMovementScript playerMovement;
    HealthBarScript healthBar;
    public float fadeDuration = 1.0f; // Duration of the fade effect
    public float holdDuration = 1.0f; // Duration to hold the screen black
    public Color fadeColor = Color.black; // Color to fade to
    public Image transitionImage; // Reference to the Image component
    public Image backgroundImage; // Reference to the Image component

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
        healthBar = GameObject.FindGameObjectWithTag("PlayerUI").GetComponentInChildren<HealthBarScript>();
        StartCoroutine(EndTranstion());
    }

    public IEnumerator EndTranstion()
    {
        // Hold black screen
        playerMovement.canMove = false;
        healthBar.UpdateHealthBarImage(PlayerStatsScript.playerHP, PlayerStatsScript.playerMaxHP);

        yield return new WaitForSeconds(holdDuration);

        // Fade out
        yield return FadeImage(transitionImage, 0f, fadeDuration / 2);
         

        yield return new WaitForSeconds(fadeDuration / 2);

        transitionImage.enabled = false;

        playerMovement.canMove = true;
    }

    public IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        Color currentColor = image.color;
        float alphaChangePerSecond = Mathf.Abs(currentColor.a - targetAlpha) / duration;

        while (!Mathf.Approximately(image.color.a, targetAlpha))
        {
            float newAlpha = Mathf.MoveTowards(image.color.a, targetAlpha, alphaChangePerSecond * Time.deltaTime);
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }
    }
}
