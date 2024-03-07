using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightSkeletonDamageScript : MonoBehaviour
{
    public PlayerStatsScript playerStatsScript;
    public knightSkeletonStats knightSkelStats;
    public EnemyStatsScript enemyStats;
    public GameObject attackTrigger;
    public Animator knightSkeletonAnimator;
    public int knightSkeletonAttackHash;
    public int knightSkeletonAttackEndHash;
    public AnimatorStateInfo animationStateInfo;

    void Start()
    {
        knightSkeletonAttackHash = Animator.StringToHash("KnightSkeletonLayer.Knight_Attack"); 
        knightSkeletonAttackEndHash = Animator.StringToHash("KnightSkeletonLayer.Knight_Attack_End");
    }
    void Update()
    {
        playerStatsScript = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStatsScript>();
        knightSkelStats = GetComponentInParent<knightSkeletonStats>();
        enemyStats = GetComponentInParent<EnemyStatsScript>();
        knightSkeletonAnimator.SetBool("isKnightSkeletonAttacking", knightSkelStats.isKnightSkeletonAttacking);
        knightSkeletonAnimator.SetBool("isPlayerTakingDamage", playerStatsScript.isPlayerTakingDamage);
        animationStateInfo = knightSkeletonAnimator.GetCurrentAnimatorStateInfo(0); //Pro zjištìní stavu animace, která zrovna hraje

        if (animationStateInfo.fullPathHash == knightSkeletonAttackEndHash && !playerStatsScript.isPlayerInvincible && knightSkelStats.isKnightSkeletonAttacking)
        {
            playerStatsScript.isPlayerTakingDamage = true;
            playerStatsScript.playerHP -= knightSkelStats.knightSkeletonDamage;
            StartCoroutine(playerStatsScript.PlayerInvicibility());
        }

        else {
            playerStatsScript.isPlayerTakingDamage = false;
        }

        if (animationStateInfo.fullPathHash == knightSkeletonAttackEndHash && knightSkelStats.isKnightSkeletonAttacking == false)
        {
            enemyStats.enemyMovementSpeed = knightSkelStats.knightSkeletonMovementSpeed;
        }

        else if (animationStateInfo.fullPathHash == knightSkeletonAttackHash || animationStateInfo.fullPathHash == knightSkeletonAttackEndHash) {
            enemyStats.enemyMovementSpeed = 0;
        }
    }
    public void AssignAttackTrigger(GameObject knightSkeletonPrefab)
    { 
        attackTrigger = knightSkeletonPrefab.transform.Find("attackTrigger").gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float originalKnightSkeletonMovementSpeed = enemyStats.enemyMovementSpeed;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            knightSkelStats.isKnightSkeletonAttacking = true;
            knightSkeletonAnimator.Play("Knight_Attack", 0, 0f);
            knightSkelStats.knightSkeletonMovementSpeed = originalKnightSkeletonMovementSpeed;
            enemyStats.enemyMovementSpeed = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exit!");
            knightSkelStats.isKnightSkeletonAttacking = false;
            
        }
    }
}
