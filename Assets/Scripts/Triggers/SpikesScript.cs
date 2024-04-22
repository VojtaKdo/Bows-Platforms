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

    //Pokud se hr�� dotkne spikes a nen� nesmrteln�, tak se spust� doSpikeDamage()
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && !PlayerStatsScript.isPlayerInvincible)
        {
            StartCoroutine(doSpikeDamage());
        }
    }

    //Pokud hr�� opust� spikes, tak se zastav� doSpikeDamage()
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            Debug.Log("Stop all courountines");
        }
    }

    //Ka�d�ch 0,5 vte�in se ubere hr��ovi spikesDamage
    IEnumerator doSpikeDamage() {
        Debug.Log("Player touched spikes!");
        PlayerStatsScript.playerHP -= spikesDamage;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(doSpikeDamage());
    }
}
