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

            horizontal = Input.GetAxis("Horizontal");   //Chozen� doprava a doleva
            playerRigidBody.velocity = new Vector2(horizontal * movementSpeed, playerRigidBody.velocity.y);
            flipCharacter();

            if (horizontal > 0 || horizontal < 0)
            {
                playerAnimator.SetFloat("playerMovementSpeed", Mathf.Abs(horizontal));  //bere hodnotu z horizontal, aby se mohl pou��t playerMovementSpeed v animatoru
            }

            if (Input.GetButtonDown("Jump") & Grounded())   //Kontrola jestli je na zemi, tak m��e sko�it
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpPower);
            }

            if (playerRigidBody.velocity.y > 3)     // kdy� hr�� ud�l� skok (nahoru), tak se zahraje animace
            {
                isJumping = true;
            }

            else if (playerRigidBody.velocity.y < -3)
            { //pi�o co j� v�m
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

    public bool Grounded() {    //vrac� hodnotu, kdy� se kruh o polom�ru 0.2f dotkne groundLayer
        return Physics2D.OverlapCircle(jumpControl.transform.position, 0.2f, groundLayer);
    }

    public void flipCharacter(){    //Funkce pro oto�en� charaktera pokud chod� doprava nebo doleva
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

        if (Grounded() && dashesAvailable >= 0 && dashesAvailable != numberOfDashes && isDashing == false && dashTrigger == false)   //Zkontroluje jestli je na zemi (Grounded()), zda m� po�et dostupn�ch dash� (dashesAvailable) men�� ne� po�et dash� (numberOfDashes), jestli zrovna dashuje (isDashing) a dashTrigger se d� false,
                                                                                                                                     //kdy� se po�k� 2 vte�iny (dashingCooldown) v dashCooldown funkci, pak se d� true a zaru��, �e se ta funkce vykon� pouze jednou
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
                for (double timer = dashingCooldown + 2; timer >= 0; timer -= Time.deltaTime)     //Ka�d� 2 vte�iny se obnov� dash
                {
                    Debug.Log(timer);
                    if (dashesAvailable != currentdashesAvailable)  //Pokud dashne v pr�b�hu �asu, co se mu obnovuje dash, tak se za�ne �as po��tat od znova
                    {
                        timer = 3;
                        dashTrigger = false;
                        yield break;
                    }
                    if (timer < 1)
                    {    //Pokud po�k� dokud prob�hne �as, tak se mu navr�t� v�echny dashe
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
