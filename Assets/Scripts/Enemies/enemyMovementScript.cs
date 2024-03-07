using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovementScript : MonoBehaviour
{
    public Rigidbody2D enemyRigidBody;
    public GameObject groundCollision;
    EnemyStatsScript enemyStats;
    public bool isFacingLeft;
    public float facingValue = 1;
    // Start is called before the first frame update
    void Start()
    {
        //Z�sk�n� scriptu a rigidBody2D pro ka�d�ho nep��tele zvl᚝
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStatsScript>();
    }

    // Update is called once per frame 
    void Update()
    {
        if (enemyRigidBody != null && enemyStats != null)
        {
            
            enemyRigidBody.velocity = new Vector2(enemyStats.enemyMovementSpeed, 0f);
        }
    }

    public void flipCharacter()
    {   //Funkce pro oto�en� charaktera pokud chod� doprava nebo doleva
        if (enemyRigidBody != null && enemyStats != null)
        {
            if (!isFacingLeft || isFacingLeft)
            {
                isFacingLeft = !isFacingLeft;
                transform.Rotate(0f, 180f, 0f);
                facingValue = -facingValue;
                enemyStats.enemyMovementSpeed = -enemyStats.enemyMovementSpeed;
            }
        }
    }
     
    public bool Grounded()
    {    //vrac� hodnotu, kdy� se kruh o polom�ru 0.2f dotkne groundLayer
        return Physics2D.OverlapCircle(groundCollision.transform.position, 0.2f, enemyStats.groundLayer);
    }
}
