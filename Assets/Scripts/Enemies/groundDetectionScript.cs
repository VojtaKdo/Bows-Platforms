using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundDetectionScript : MonoBehaviour
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.layer == 6 && !collision.gameObject.CompareTag("Border") && !collision.isTrigger && !collision.gameObject.CompareTag("OutOfBounds"))
        {
            enemyMovement.flipCharacter();
        }
    }
}
