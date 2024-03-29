using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetectionScript : MonoBehaviour
{
    enemyMovementScript enemyMovement;
    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponentInParent<enemyMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if ((!collision.gameObject.CompareTag("Arrow") || collision.gameObject.CompareTag("Border")) && !collision.isTrigger && !collision.gameObject.CompareTag("OutOfBounds"))
        {
            enemyMovement.flipCharacter();
        }
    }
}
