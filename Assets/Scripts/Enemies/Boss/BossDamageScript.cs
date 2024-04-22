using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossDamageScript : MonoBehaviour
{
    public PlayerStatsScript playerStatsScript;
    public BossStatsScript bossStats;
    public EnemyStatsScript enemyStats;
    public enemyMovementScript enemyMovement;
    public HealthBarScript healthBar;
    public GameObject attackTrigger;
    public Animator bossAnimator;
    public GameObject collisionDetection;
    public GameObject Summons;
    public GameObject summonsSpawn;
    public int bossAttackHash;
    public int bossAttackDamageHash;
    public int bossAttackEndHash;
    public int bossWalkingHash;
    public int bossSummonHash;
    public AnimatorStateInfo animationStateInfo;

    void Start()
    {
        bossAttackHash = Animator.StringToHash("BossLayer.Boss_Attacking");
        bossAttackDamageHash = Animator.StringToHash("BossLayer.Boss_Attacking_Damage");
        bossAttackEndHash = Animator.StringToHash("BossLayer.Boss_Attacking_End");
        bossWalkingHash = Animator.StringToHash("BossLayer.Boss_Walking");
        bossSummonHash = Animator.StringToHash("BossLayer.Boss_Summon");
        enemyMovement = GetComponentInParent<enemyMovementScript>();
    }
    void Update()
    {
        playerStatsScript = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStatsScript>();
        healthBar = GameObject.FindGameObjectWithTag("PlayerUI")?.GetComponentInChildren<HealthBarScript>();
        bossStats = GetComponentInParent<BossStatsScript>();
        enemyStats = GetComponentInParent<EnemyStatsScript>();
        bossAnimator.SetBool("isBossAttacking", enemyStats.isEnemyAttacking);
        bossAnimator.SetBool("isBossSummoning", bossStats.isSummoning);
        animationStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0); //Pro zjištìní stavu animace, která zrovna hraje

        //Pokud nepøítel dokonèí útok a hráè není nesmrtelný, tak udìlá knightSkeletonDamage hráèi
        if (animationStateInfo.fullPathHash == bossAttackDamageHash && !PlayerStatsScript.isPlayerInvincible && enemyStats.isEnemyAttacking && !bossStats.isSummoning)
        {
            Debug.Log("Boss is attacking!");
            PlayerStatsScript.playerHP -= bossStats.bossDamage;
        }

        if (bossStats.bossHP <= 35 && !bossStats.isSummoning && bossStats.summonOnce) {
            enemyStats.enemyMovementSpeed *= 1.25f;
            StartCoroutine(Summon());
        }

        if (bossStats.bossHP <= 10 && !bossStats.isSummoning && bossStats.finalPhaseOnce) {
            enemyStats.enemyMovementSpeed *= 1.5f;
            bossStats.summonCooldown = 2f;
            bossStats.finalPhaseOnce = false;
        }

        else if (animationStateInfo.fullPathHash == bossAttackHash || animationStateInfo.fullPathHash == bossAttackDamageHash || animationStateInfo.fullPathHash == bossAttackEndHash)
        {
            enemyMovement.enemyRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        if (animationStateInfo.fullPathHash == bossWalkingHash)
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
        if (collision.gameObject.CompareTag("Player") && !bossStats.isSummoning)
        {
            Debug.Log("Player hit!");
            enemyStats.isEnemyAttacking = true;
            bossAnimator.Play("Boss_Attacking", 0, 0f);
        }
    }

    //Pokud hráèùv damageHitbox vyleze z attackTriggeru, tak pøestane nepøítel útoèit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("getDamageHitbox") && !bossStats.isSummoning)
        {
            Debug.Log("Player exit!");
            enemyStats.isEnemyAttacking = false;
        }
    }

    IEnumerator Summon() {
        bossStats.summonOnce = false;
        Debug.Log("Boss is summoning!");
        yield return new WaitForSeconds(bossStats.summonCooldown);
        bossStats.isSummoning = true;
        float enemyOriginalMovementSpeed = enemyStats.enemyMovementSpeed;
        bossAnimator.Play("Boss_Summon", 0, 0f);
        Instantiate(Summons, new Vector3(summonsSpawn.transform.position.x, summonsSpawn.transform.position.y, 0), Quaternion.identity);
        enemyStats.enemyMovementSpeed = 0f;
        yield return new WaitForSeconds(bossStats.summonTime);
        bossStats.isSummoning = false;
        enemyStats.enemyMovementSpeed = enemyOriginalMovementSpeed;
        StartCoroutine(Summon());
    }
}
