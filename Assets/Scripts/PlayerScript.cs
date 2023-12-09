using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Movement
    public float movementSpeed = 10;
    public float jumpPower = 5;
    public float dashPower = 10;
    public float dashingTime = 2;
    public float dashingCooldown = 2;
    public float numberOfDashes = 1;
    public float dashesAvailable = 1;
    private float horizontal;
    public double jumpingTime = 0.45;
    private bool isFacingRight;
    public bool isJumping;
    public bool isDashing;
    public bool dashTrigger;
    public bool cancelDashTrigger;
    public bool canDash = true;
    

    //Respawn
    public float deadzone = -10;
    public GameObject respawnZone;

    public Rigidbody2D playerRigidBody;
    public LayerMask groundLayer;
    public GameObject jumpControl;
    public Animator playerAnimator;

    // Update is called once per frame
    void Update()
    {
        //Camera
        Camera playerCamera = Camera.main;
        playerCamera.transform.position = new Vector3(playerRigidBody.position.x, playerRigidBody.position.y, -10); //Pozice kamery

        //Movement
        if (playerAnimator != null)
        {
            if (isDashing)
            {
                return;
            }

            horizontal = Input.GetAxis("Horizontal");   //Chození doprava a doleva
            playerRigidBody.velocity = new Vector2(horizontal * movementSpeed, playerRigidBody.velocity.y);
            flipCharacter();

            if (horizontal > 0 || horizontal < 0)
            {
                playerAnimator.SetFloat("playerMovementSpeed", Mathf.Abs(horizontal));  //bere hodnotu z horizontal, aby se mohl použít playerMovementSpeed v animatoru
            }

            if (Input.GetButtonDown("Jump") & Grounded())   //Kontrola jestli je na zemi, tak mùže skoèit
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpPower);
            }

            if (playerRigidBody.velocity.y > 3)     // když hráè udìlá skok (nahoru), tak se zahraje animace
            {
                isJumping = true;
            }

            else if (playerRigidBody.velocity.y < -3)
            { //pièo co já vím
                isJumping = false;
            }

            playerAnimator.SetBool("isJumping", isJumping);
            playerAnimator.SetBool("PlayerOnGround", Grounded());
        }

        dashFunction();
        //Respawn
        if (playerRigidBody.position.y < deadzone) {
            playerRigidBody.position = respawnZone.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) {
            return;
        }
    }

    public bool Grounded() {    //vrací hodnotu, když se kruh o polomìru 0.2f dotkne groundLayer
        return Physics2D.OverlapCircle(jumpControl.transform.position, 0.2f, groundLayer);
    }

    public void flipCharacter(){    //Funkce pro otoèení charaktera pokud chodí doprava nebo doleva
        if (isFacingRight && horizontal > 0f || !isFacingRight && horizontal < 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }

    public void dashFunction()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true && dashesAvailable != 0)
        {
            StartCoroutine(Dash());
        }

        if (Grounded() && dashesAvailable >= 0 && dashesAvailable != numberOfDashes && isDashing == false && dashTrigger == false)   //Zkontroluje jestli je na zemi (Grounded()), zda má poèet dostupných dashù (dashesAvailable) menší než poèet dashù (numberOfDashes), jestli zrovna dashuje (isDashing) a dashTrigger se dá false,
                                                                                                                                     //když se poèká 2 vteøiny (dashingCooldown) v dashCooldown funkci, pak se dá true a zaruèí, že se ta funkce vykoná pouze jednou
        {
            dashTrigger = true;
            StartCoroutine(dashCooldown());
        }

        IEnumerator Dash()
        {
            isDashing = true;
            canDash = false;
            dashesAvailable = dashesAvailable + (dashesAvailable - (dashesAvailable + 1));
            float originalGravity = playerRigidBody.gravityScale;
            playerRigidBody.gravityScale = 0;
            playerRigidBody.velocity = new Vector2(dashPower * transform.localScale.x, 0);
            yield return new WaitForSeconds(dashingTime);
            playerRigidBody.gravityScale = originalGravity;
            isDashing = false;
            if (dashesAvailable > 0)
            {
                canDash = true;
            }
            else if (dashesAvailable == 0)
            {
                canDash = false;
            }
        }

        IEnumerator dashCooldown()
        {
            float currentdashesAvailable = dashesAvailable;
            {
                for (double timer = dashingCooldown + 2; timer >= 0; timer -= Time.deltaTime)     //Každý 2 vteøiny se obnoví dash
                {
                    Debug.Log(timer);
                    if (dashesAvailable != currentdashesAvailable)  //Pokud dashne v prùbìhu èasu, co se mu obnovuje dash, tak se zaène èas poèítat od znova
                    {
                        timer = 3;
                        dashTrigger = false;
                        yield break;
                    }
                    if (timer < 1)
                    {    //Pokud poèká dokud probìhne èas, tak se mu navrátí všechny dashe
                        dashesAvailable = numberOfDashes;
                        dashTrigger = false;
                        canDash = true;
                        yield break;
                    }
                    yield return null;
                }
            }
        }
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isDashing", isDashing);
        }
    }
}
