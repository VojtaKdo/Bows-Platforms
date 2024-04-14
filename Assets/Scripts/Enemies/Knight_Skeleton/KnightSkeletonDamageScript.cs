using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightSkeletonDamageScript : MonoBehaviour
{
    public PlayerStatsScript playerStatsScript;
    public knightSkeletonStats knightSkelStats;
    public EnemyStatsScript enemyStats;
    public enemyMovementScript enemyMovement;
    public HealthBarScript healthBar;
    public GameObject attackTrigger;
    public Animator knightSkeletonAnimator;
    public GameObject collisionDetection;
    public int knightSkeletonAttackHash;
    public int knightSkeletonAttackEndHash;
    public int knightSkeletonWalkingHash;
    public AnimatorStateInfo animationStateInfo;

    void Start()
    {
        knightSkeletonAttackHash = Animator.StringToHash("KnightSkeletonLayer.Knight_Attack"); 
        knightSkeletonAttackEndHash = Animator.StringToHash("KnightSkeletonLayer.Knight_Attack_End");
        knightSkeletonWalkingHash = Animator.StringToHash("KnightSkeletonLayer.Knight_Walking");
        enemyMovement = GetComponentInParent<enemyMovementScript>();
    }
    void Update()
    { 
        playerStatsScript = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStatsScript>();
        healthBar = GameObject.FindGameObjectWithTag("PlayerUI")?.GetComponentInChildren<HealthBarScript>();
        knightSkelStats = GetComponentInParent<knightSkeletonStats>();
        enemyStats = GetComponentInParent<EnemyStatsScript>();
        knightSkeletonAnimator.SetBool("isKnightSkeletonAttacking", enemyStats.isEnemyAttacking);
        animationStateInfo = knightSkeletonAnimator.GetCurrentAnimatorStateInfo(0); //Pro zjištìní stavu animace, která zrovna hraje

        //Pokud nepøítel dokonèí útok a hráè není nesmrtelný, tak udìlá knightSkeletonDamage hráèi
        if (animationStateInfo.fullPathHash == knightSkeletonAttackEndHash && !playerStatsScript.isPlayerInvincible && enemyStats.isEnemyAttacking)
        {
            Debug.Log("Skeleton is attacking!");
            PlayerStatsScript.playerHP -= knightSkelStats.knightSkeletonDamage;
        }

        else if (animationStateInfo.fullPathHash == knightSkeletonAttackHash || animationStateInfo.fullPathHash == knightSkeletonAttackEndHash) {
            enemyMovement.enemyRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        if (animationStateInfo.fullPathHash == knightSkeletonWalkingHash)
        {
            enemyMovement.enemyRigidBody.constraints = ~RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        enemyMovement.enemyRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }
    public void AssignAttackTrigger(GameObject knightSkeletonPrefab)
    { 
        attackTrigger = knightSkeletonPrefab.transform.Find("attackTrigger").gameObject;
    }

    //Pokud hráè vleze do attackTriggeru, tak zaène nepøítel útoèit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            enemyStats.isEnemyAttacking = true;
            knightSkeletonAnimator.Play("Knight_Attack", 0, 0f);
        }
    }

    //Pokud hráèùv damageHitbox vyleze z attackTriggeru, tak pøestane nepøítel útoèit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("getDamageHitbox"))
        {
            Debug.Log("Player exit!");
            enemyStats.isEnemyAttacking = false;
        }
    }
}
