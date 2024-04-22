using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    public BossStatsScript bossStats;
    public Animator summonsAnimator;
    public float summonsMovementSpeed = 3f;
    public float summonsDamage = 1f;
    public float summonChangePositonCooldown = 2f;
    public Rigidbody2D summonsRigidBody;
    public GameObject player;

    void Start()
    {
        summonsAnimator.Play("Summon_Spawn", 0, 0f);
        StartCoroutine(SummonsMovement());
        player = GameObject.FindGameObjectWithTag("Player");
        bossStats = GameObject.FindGameObjectWithTag("Boss").GetComponentInParent<BossStatsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossStats.bossHP <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator SummonsMovement() {
        while (true) // Loop indefinitely
        {
            // Wait for 5 seconds before each movement
            yield return new WaitForSeconds(summonChangePositonCooldown);

            // Get the position of the player
            Vector3 targetPosition = player.transform.position;

            // Move the summons towards the player
            yield return MoveToPosition(targetPosition);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        // Calculate the distance to move
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Continue moving until the distance is negligible
        while (distance > 0.1f)
        {
            // Move towards the target position gradually
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * summonsMovementSpeed);

            // Recalculate the distance
            distance = Vector3.Distance(transform.position, targetPosition);

            // Wait for the next frame
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Player touched!");
            PlayerStatsScript.playerHP -= summonsDamage;
        }
    }
}
