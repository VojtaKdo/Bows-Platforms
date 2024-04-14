using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalTriggerScript : MonoBehaviour
{
    public SceneStartTransitionControllerScript SceneStartTransitionController;
    public GameObject Portal;

    void Start()
    {
        Portal.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && SceneStartTransitionController != null) {
            SceneStartTransitionController.transitionImage.enabled = true;
            PlayButtonScript.Level += 1;
            StartCoroutine(SceneStartTransitionController.StartTransition());
        }
    }
}
