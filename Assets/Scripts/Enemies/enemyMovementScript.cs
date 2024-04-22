using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovementScript : MonoBehaviour
{
    public Rigidbody2D enemyRigidBody;
    EnemyStatsScript enemyStats;
    public bool isFacingLeft;
    public float facingValue = 1;
    //public float previousPositionY;
    // Start is called before the first frame update
    void Start()
    {
        //Z�sk�n� scriptu a rigidBody2D pro ka�d�ho nep��tele zvl᚝
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStatsScript>();

        //previousPositionY = transform.position.y;
    }

    // Update is called once per frame 
    void Update()
    {
        //float currentPositionY = transform.position.y;

        
        if (enemyRigidBody != null && enemyStats != null)
        {
            
            enemyRigidBody.velocity = new Vector2(enemyStats.enemyMovementSpeed, 0f);

            //Zv�t�uje se pozice Y
           /* if (currentPositionY > previousPositionY)
            {
                enemyRigidBody.gravityScale = 1f;
            }

            //Zmen�uje se pozice Y
            else if (currentPositionY < previousPositionY)
            {
                enemyRigidBody.gravityScale = 15f;
            }

            previousPositionY = currentPositionY;*/
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
}
