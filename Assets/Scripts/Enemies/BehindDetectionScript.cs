using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindDetectionScript : MonoBehaviour
{
    public enemyMovementScript enemyMovement;
    public BossStatsScript bossStats;
    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponentInParent<enemyMovementScript>();
        bossStats = GetComponentInParent<BossStatsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !bossStats.isSummoning)
        {
            Debug.Log("There is a player behind you!");
            enemyMovement.flipCharacter();

        }
    }
}
