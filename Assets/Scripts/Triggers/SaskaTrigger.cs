using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaskaTrigger : MonoBehaviour
{
    public AudioManagerScript audioManager;
    public GameObject Saska;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponentInParent<AudioManagerScript>();
        Saska.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Saska.SetActive(true);
            audioManager.PlaySFX(audioManager.saskaScream);
            audioManager.PlaySFX(audioManager.saskaScream);
            audioManager.PlaySFX(audioManager.saskaScream);
            Destroy(this);
        }
    }
}
