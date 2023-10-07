using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float movementSpeed = 10;
    public float jumpPower = 5;
    public Rigidbody2D playerRigidBody;
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Camera
        Camera playerCamera = Camera.main;
        playerCamera.transform.position = new Vector3(playerRigidBody.position.x, playerRigidBody.position.y, -10);

        horizontal = Input.GetAxis("Horizontal");
        playerRigidBody.velocity = new Vector2(horizontal * movementSpeed, playerRigidBody.velocity.y);

        if (Input.GetButtonDown("Jump")) {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpPower);
        }
    }
}
