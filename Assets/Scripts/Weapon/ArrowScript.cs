using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public Rigidbody2D arrowRigidBody;
    public float arrowSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        arrowRigidBody.velocity = transform.right * arrowSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        if (!hitInfo.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
