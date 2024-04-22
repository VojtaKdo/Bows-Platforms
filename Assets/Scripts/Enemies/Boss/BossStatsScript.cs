using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatsScript : MonoBehaviour
{
    public AudioManagerScript audioManager;
    public float bossHP = 50;
    public float bossMaxHP = 50;
    public float bossDamage = 2;
    public float summonTime = 1f;
    public float summonBallsTime = 0.5f;
    public float summonCooldown = 5f;
    public bool summonOnce = true;
    public bool finalPhaseOnce = true;
    public bool isSummoning;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponentInParent<AudioManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHP <= 0)
        {
            Destroy(gameObject);
            audioManager.StopMusic();
        }
    }
}
