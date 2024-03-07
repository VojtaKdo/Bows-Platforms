using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyTriggerScript : MonoBehaviour
{
    public GameObject spawnEnemy;
    public GameObject knightSkeletonPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Trigger na spawnování nepøátelù
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy Spawned!");
            GameObject newKnightSkeleton = SpawnEnemy(knightSkeletonPrefab);

            KnightSkeletonDamageScript knightSkeletonDMGscript = newKnightSkeleton.GetComponent<KnightSkeletonDamageScript>();

            if (knightSkeletonDMGscript != null) {
                knightSkeletonDMGscript.AssignAttackTrigger(newKnightSkeleton);
            }
            //Destroy(this);
            //Destroy(gameObject);
        }
    }

    //Spawnování nepøítele (pro teï Warrior skeletona)
    private GameObject SpawnEnemy(GameObject enemy) {
        //Pokud enemy neexistuje, tak se to vykoná
        if (enemy != null)
        {
            //Spawne se nìpøítel v pøedurèeném rozmezí a uloží se do newKnightSkeleton
            GameObject newKnightSkeleton = Instantiate(enemy, new Vector3(Random.Range(spawnEnemy.transform.position.x + 5f, spawnEnemy.transform.position.x - 5f), Random.Range(spawnEnemy.transform.position.y, spawnEnemy.transform.position.y), 0), Quaternion.identity);

            //Získáme scripty pro každého nepøítele vlastní
            knightSkeletonStats knightSkelStats = enemy.GetComponent<knightSkeletonStats>();
            EnemyStatsScript enemyStats = enemy.GetComponent<EnemyStatsScript>();


            //Pøídáme každému nepøíteli svoje vlastní vlastnosti
            if (knightSkelStats != null && enemyStats != null)
            {
                enemyStats.enemyHP = knightSkelStats.knightSkeletonHP;
                enemyStats.enemyMaxHP = knightSkelStats.knightSkeletonMaxHP;
                enemyStats.enemyMovementSpeed = knightSkelStats.knightSkeletonMovementSpeed;
                enemyStats.enemyDamage = knightSkelStats.knightSkeletonDamage;
            }
            return newKnightSkeleton;
        }

        else {
            return null;
        }
    }
}
