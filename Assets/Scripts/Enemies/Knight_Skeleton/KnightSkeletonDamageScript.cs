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
        animationStateInfo = knightSkeletonAnimator.GetCurrentAnimatorStateInfo(0); //Pro zji�t�n� stavu animace, kter� zrovna hraje

        //Pokud nep��tel dokon�� �tok a hr�� nen� nesmrteln�, tak ud�l� knightSkeletonDamage hr��i
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

    //Pokud hr�� vleze do attackTriggeru, tak za�ne nep��tel �to�it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            enemyStats.isEnemyAttacking = true;
            knightSkeletonAnimator.Play("Knight_Attack", 0, 0f);
        }
    }

    //Pokud hr���v damageHitbox vyleze z attackTriggeru, tak p�estane nep��tel �to�it
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("getDamageHitbox"))
        {
            Debug.Log("Player exit!");
            enemyStats.isEnemyAttacking = false;
        }
    }
}
