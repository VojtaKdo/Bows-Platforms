using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    PlayerStatsScript playerStats;
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
        if (hitInfo.gameObject.CompareTag("Player") || hitInfo.isTrigger)
        {
            isDestroyed = false;
        }
        else {
            OnDestroy();
            Destroy(gameObject);
            isDestroyed = true;
        }

        if (hitInfo.gameObject.CompareTag("knightSkeleton")) {
            Debug.Log("Enemy Hit!");

            var knightSkelStats = hitInfo.gameObject.GetComponent<knightSkeletonStats>();

            if (knightSkelStats)
            {
               knightSkelStats.knightSkeletonHP -= playerStats.playerDamage; 
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
