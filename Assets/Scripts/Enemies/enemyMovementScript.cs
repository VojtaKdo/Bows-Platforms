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
        //ZÌsk·nÌ scriptu a rigidBody2D pro kaûdÈho nep¯Ìtele zvl·öù
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

            //ZvÏtöuje se pozice Y
           /* if (currentPositionY > previousPositionY)
            {
                enemyRigidBody.gravityScale = 1f;
            }

            //Zmenöuje se pozice Y
            else if (currentPositionY < previousPositionY)
            {
                enemyRigidBody.gravityScale = 15f;
            }

            previousPositionY = currentPositionY;*/
        }
    }

    public void flipCharacter()
    {   //Funkce pro otoËenÌ charaktera pokud chodÌ doprava nebo doleva
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
