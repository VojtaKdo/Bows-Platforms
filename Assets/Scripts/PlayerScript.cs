using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float movementSpeed = 10;
    public float jumpPower = 5;
    public float deadzone = -10;
    private float horizontal;
    private bool isFacingRight;
    public bool isJumping;
    public double jumpingTime = 0.45;

    public Rigidbody2D playerRigidBody;
    public GameObject respawnZone;
    public LayerMask groundLayer;
    public GameObject jumpControl;
    public Animator playerAnimator;

    public CharacterController controller;
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

        //Movement
        if (playerAnimator != null)
        {
            horizontal = Input.GetAxis("Horizontal");
            playerRigidBody.velocity = new Vector2(horizontal * movementSpeed, playerRigidBody.velocity.y);
            flipCharacter();

            if (horizontal > 0 || horizontal < 0)
            {
                playerAnimator.SetFloat("playerMovementSpeed", Mathf.Abs(horizontal));
            }

            if (Input.GetButtonDown("Jump") & Grounded())   //Kontrola jestli je na zemi, tak mùže skoèit
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpPower);
            }

            if (playerRigidBody.velocity.y > 3)     // když hráè udìlá skok (nahoru), tak se zahraje animace
            {
                isJumping = true;
                Debug.Log("Jumped");
            }

            else if (playerRigidBody.velocity.y < -3) {
                isJumping = false;
                Debug.Log("Fell");
            }

            playerAnimator.SetBool("isJumping", isJumping);
            playerAnimator.SetBool("PlayerOnGround", Grounded());
        }

        //Respawn
        if (playerRigidBody.position.y < deadzone) {
            playerRigidBody.position = respawnZone.transform.position;
        }
    }

    public bool Grounded() {
        return Physics2D.OverlapCircle(jumpControl.transform.position, 0.2f, groundLayer);
    }

    public void flipCharacter(){
        if (isFacingRight && horizontal > 0f || !isFacingRight && horizontal < 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }
}
