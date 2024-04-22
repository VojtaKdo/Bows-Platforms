using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    PlayerStatsScript playerStats;
    AudioManagerScript audioManager;
    public GameObject knightSkeleton;
    public float playerDamage = 1;
    public event Action OnDestroyed;
    public Rigidbody2D arrowRigidBody;
    public float arrowSpeed = 5;
    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();

        if (playerStats != null)
        {
            if (arrowRigidBody != null)
            {
                arrowRigidBody.velocity = transform.right * arrowSpeed;
            }
            else
            {
                Debug.LogError("ArrowScript: Rigidbody2D reference is null.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player") || hitInfo.isTrigger || hitInfo.gameObject.CompareTag("Border"))
        {
            isDestroyed = false;
        }
        else
        {
            OnDestroy();
            Destroy(gameObject);
            isDestroyed = true;
        }

        if (hitInfo.gameObject.layer == 7) {
            Debug.Log("Enemy Hit!");

            var knightSkelStats = hitInfo.gameObject.GetComponent<knightSkeletonStats>();
            var bossStats = hitInfo.gameObject.GetComponent<BossStatsScript>();
            var healthBar = hitInfo.gameObject.GetComponentInChildren<HealthBarScript>();

            if (knightSkelStats && healthBar)
            {
               knightSkelStats.knightSkeletonHP -= playerStats.playerDamage;
               healthBar.UpdateHealthBarSlider(knightSkelStats.knightSkeletonHP, knightSkelStats.knightSkeletonMaxHP);
            }

            else if (bossStats && healthBar) {
                Debug.Log("Boss hit!");
                bossStats.bossHP -= playerStats.playerDamage;
                healthBar.UpdateHealthBarSlider(bossStats.bossHP, bossStats.bossMaxHP);
            }
        }
        
    }

    private void OnDestroy()
    {
        if (!isDestroyed && OnDestroyed != null)
        {
            OnDestroyed.Invoke();
        }
    }

    // Metoda pro odstranìní skriptu
    public void RemoveArrowScript()
    {
        arrowRigidBody.velocity = Vector2.zero; 
        Destroy(this);
    }
}
