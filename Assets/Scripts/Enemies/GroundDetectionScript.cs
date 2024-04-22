using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public enemyMovementScript enemyMovement;

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
        if (collision.gameObject.layer == 6)
        {
            enemyMovement.flipCharacter();
        }
    }
}
