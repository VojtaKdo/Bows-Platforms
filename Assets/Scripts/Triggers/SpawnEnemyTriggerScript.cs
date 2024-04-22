using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemyTriggerScript : MonoBehaviour
{
    HealthBarScript healthBar;
    public AudioManagerScript audioManager;
    public GameObject spawnEnemy;
    public GameObject enemyPrefab;
    public GameObject[] numberOfEnemies;
    public float spawnRange;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponentInParent<AudioManagerScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Trigger na spawnování nepøátelù
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy Spawned!");
            GameObject enemy = SpawnEnemy(enemyPrefab);

            KnightSkeletonDamageScript knightSkeletonDMGscript = enemy.GetComponent<KnightSkeletonDamageScript>();
            BossDamageScript bossKnightDMGScript = enemy.GetComponent<BossDamageScript>();

            if (knightSkeletonDMGscript != null)
            {
                knightSkeletonDMGscript.AssignAttackTrigger(enemy);
            }
            if (bossKnightDMGScript != null)
            {
                bossKnightDMGScript.AssignAttackTrigger(enemy);
            }

            if (PlayerPrefs.GetInt("Level") == 4)
            {
                Debug.Log("Boss music!");
                audioManager.StopMusic();
                audioManager.PlayMusic(audioManager.bossMusic);
            }

            Destroy(this);
            Destroy(gameObject);
        }
    }

    //Spawnování nepøítele (pro teï Warrior skeletona)
    private GameObject SpawnEnemy(GameObject enemy)
    {
        //Pokud enemy neexistuje, tak se to vykoná
        if (enemy != null)
        {
            //Spawne se nìpøítel v pøedurèeném rozmezí a uloží se do newKnightSkeleton
            foreach (GameObject enemies in numberOfEnemies)
            {
                GameObject newEnemy = Instantiate(enemies, new Vector3(Random.Range(spawnEnemy.transform.position.x + spawnRange, spawnEnemy.transform.position.x - spawnRange), Random.Range(spawnEnemy.transform.position.y, spawnEnemy.transform.position.y), 0), Quaternion.identity);
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

        else
        {
            return null;
        }
    }
}