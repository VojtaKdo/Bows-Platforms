using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerStatsScript playerStats;
    public HealthBarScript healthBar;
    public float spikesDamage;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsScript>();
        healthBar = GameObject.FindGameObjectWithTag("PlayerUI").GetComponentInChildren<HealthBarScript>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Pokud se hráè dotkne spikes a není nesmrtelný, tak se spustí doSpikeDamage()
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && !PlayerStatsScript.isPlayerInvincible)
        {
            StartCoroutine(doSpikeDamage());
        }
    }

    //Pokud hráè opustí spikes, tak se zastaví doSpikeDamage()
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            Debug.Log("Stop all courountines");
        }
    }

    //Každých 0,5 vteøin se ubere hráèovi spikesDamage
    IEnumerator doSpikeDamage() {
        Debug.Log("Player touched spikes!");
        PlayerStatsScript.playerHP -= spikesDamage;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(doSpikeDamage());
    }
}
