using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemyTriggerScript : MonoBehaviour
{
    HealthBarScript healthBar;
    public GameObject spawnEnemy;
    public GameObject knightSkeletonPrefab;
    public GameObject[] numberOfEnemies;
    public float spawnRange;

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
            Destroy(this);
            Destroy(gameObject);
        }
    }

    //Spawnování nepøítele (pro teï Warrior skeletona)
    private GameObject SpawnEnemy(GameObject enemy) {
        //Pokud enemy neexistuje, tak se to vykoná
        if (enemy != null)
        {
            //Spawne se nìpøítel v pøedurèeném rozmezí a uloží se do newKnightSkeleton
            foreach (GameObject enemies in numberOfEnemies)
            {
                GameObject newKnightSkeleton = Instantiate(enemies, new Vector3(Random.Range(spawnEnemy.transform.position.x + spawnRange, spawnEnemy.transform.position.x - spawnRange), Random.Range(spawnEnemy.transform.position.y, spawnEnemy.transform.position.y), 0), Quaternion.identity);
            }

            //Získáme scripty pro každého nepøítele vlastní
            knightSkeletonStats knightSkelStats = enemy.GetComponent<knightSkeletonStats>();
            EnemyStatsScript enemyStats = enemy.GetComponent<EnemyStatsScript>();


            //Pøídáme každému nepøíteli svoje vlastní vlastnosti
            if (knightSkelStats != null && enemyStats != null && healthBar != null)
            {
                enemyStats.enemyHP = knightSkelStats.knightSkeletonHP;
                enemyStats.enemyMaxHP = knightSkelStats.knightSkeletonMaxHP;
                enemyStats.enemyDamage = knightSkelStats.knightSkeletonDamage;
            }
            return enemy;
        }

        else {
            return null;
        }
    }
}
