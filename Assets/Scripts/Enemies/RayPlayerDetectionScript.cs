using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPlayerDetectionScript : MonoBehaviour
{
    public enemyMovementScript enemyMovement;
    public EnemyStatsScript enemyStats;
    public knightSkeletonStats knightSkelStats;
    public BoxCollider2D behindSkeletonPlayerDetectionBoxCollider;
    public GameObject rayPlayerDetectionGameObject;
    public GameObject groundDetection;
    public LayerMask detectionLayer;
    public float range = 8;
    public Animator knightSkeletonAnimator;
    public AnimatorStateInfo animationStateInfo;
    public int knightSkeletonWalkingHash;
    public float rayOffset = 2;
    public bool isPlayerVisible;
    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponentInParent<enemyMovementScript>();
        enemyStats = GetComponentInParent<EnemyStatsScript>();
        knightSkelStats = GetComponentInParent<knightSkeletonStats>();
        behindSkeletonPlayerDetectionBoxCollider = GetComponentInChildren<BoxCollider2D>();
        knightSkeletonWalkingHash = Animator.StringToHash("KnightSkeletonLayer.Knight_Walking");
    }

    // Update is called once per frame
    void Update()
    {
        animationStateInfo = knightSkeletonAnimator.GetCurrentAnimatorStateInfo(0); //Pro zji�t�n� stavu animace, kter� zrovna hraje

        //Nep��tel za�ne vyza�ovat Raycast, kter� se p�izp�sobuje rychlosti pohybu nep��tele
        RaycastHit2D rayPlayerDetection = Physics2D.Raycast(rayPlayerDetectionGameObject.transform.position, range * new Vector2(enemyMovement.facingValue, 0f), range, detectionLayer);

        //Pokud je n�co v collideru a nep��tel ne�to��, tak se vykresl� �erven� paprsek (Ray)
        if (rayPlayerDetection.collider != null && !enemyStats.isEnemyAttacking)
        {
            Debug.DrawRay(rayPlayerDetectionGameObject.transform.position, range * new Vector2(enemyMovement.facingValue, 0f), Color.red);

            //Pokud nep��tel chod� a aktivovalo se mu agro, tak se mu zdvojn�sob� rychlost pohybu
            if (animationStateInfo.fullPathHash == knightSkeletonWalkingHash && knightSkelStats.agroOnce)
            {
                enemyStats.enemyMovementSpeed = enemyStats.enemyMovementSpeed * 2;
                knightSkelStats.agroOnce = false;
            }
        }


        else
        {
            //Debug.Log("No Player!");

            //Pokud nikdo nen� v collideru, tak se mu deaktivuje agro a vykresl� se zelen paprsek (Ray)
            knightSkelStats.agroOnce = true;
            Debug.DrawRay(rayPlayerDetectionGameObject.transform.position, range * new Vector2(enemyMovement.facingValue, 0f), Color.green);
            if ((enemyMovement.isFacingLeft && animationStateInfo.fullPathHash == knightSkeletonWalkingHash) || (enemyMovement.isFacingLeft && animationStateInfo.fullPathHash == knightSkeletonWalkingHash))
            {
                enemyStats.enemyMovementSpeed = -3;
            }
            else if ((!enemyMovement.isFacingLeft && !enemyStats.isEnemyAttacking) || (!enemyMovement.isFacingLeft && animationStateInfo.fullPathHash == knightSkeletonWalkingHash))
            {
                enemyStats.enemyMovementSpeed = 3;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("There is a player behind you!");
            enemyMovement.flipCharacter();
            
        }
    }
}
