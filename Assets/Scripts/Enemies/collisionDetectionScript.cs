using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetectionScript : MonoBehaviour
{
    enemyMovementScript enemyMovement;
    // Start is called before the first frame update
    public EnemyStatsScript enemyStats;
    public Vector2 boxSize = new Vector2(1f, 1f);
    public LayerMask groundLayer;
    public Animator enemyAnimator;
    public int knightSkeletonWalkingHash;
    public int bossWalkingHash;
    public AnimatorStateInfo animationStateInfo;

    void Start()
    {
        enemyMovement = GetComponentInParent<enemyMovementScript>();
        enemyStats = GetComponentInParent<EnemyStatsScript>();
        knightSkeletonWalkingHash = Animator.StringToHash("KnightSkeletonLayer.Knight_Walking");
        bossWalkingHash = Animator.StringToHash("BossLayer.Boss_Walking");
    }

    // Update is called once per frame
    void Update()
    {
        animationStateInfo = enemyAnimator.GetCurrentAnimatorStateInfo(0); //Pro zji�t�n� stavu animace, kter� zrovna hraje

        //Vykresl� box a ulo�� v�echno, �eho se dotkne, do colliders
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, groundLayer);

        DebugDrawBox(transform.position, boxSize, Color.blue);

        //Zjist�, jestli tam jsou n�jak� collidery, pokud se ni�eho nedot�k�(vzduch)
        if (colliders.Length == 0 && !enemyStats.isEnemyAttacking && (animationStateInfo.fullPathHash == knightSkeletonWalkingHash || animationStateInfo.fullPathHash == bossWalkingHash))
        {
            enemyMovement.flipCharacter();
        }
    }

    // Pomocn� metoda pro vykreslen� boxu ve sc�n�
    private void DebugDrawBox(Vector2 center, Vector2 size, Color color)
    {
        Vector2 halfSize = size / 2;
        Vector2 topLeft = center - halfSize;
        Vector2 bottomRight = center + halfSize;
        Vector2 topRight = new Vector2(bottomRight.x, topLeft.y);
        Vector2 bottomLeft = new Vector2(topLeft.x, bottomRight.y);

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }




}
