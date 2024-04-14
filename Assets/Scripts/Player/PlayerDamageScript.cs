using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamageScript : MonoBehaviour
{
    PlayerMovementScript playerMovement;
    PlayerStatsScript playerStats;
    AudioManagerScript audioManager;
    public GameObject firePoint;
    public GameObject Arrow;
    private GameObject spawnedArrow;
    public bool isShooting = false;
    public bool isChargingShot = false;
    public bool isFullyCharged = false;
    public bool canShootAirborne = false;
    public bool playSFXonce = false;
    public float fullyChargedCooldown = 0.5f;
    public float shootingCooldown = 0.6f;
    public float airShots = 2;
    public float airShotsAvailable = 2;
    public int normalAttackHash;
    public int chargingShotHash;
    public int airborneAttackHash;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsScript>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        normalAttackHash = Animator.StringToHash("PlayerAnimationLayer.Normal_Attack_Loop"); //P�evede to animaci Normal_Attack_Loop do unik�tn�ho hashe 
        chargingShotHash = Animator.StringToHash("PlayerAnimationLayer.Normal_Attack_Beginning");
        airborneAttackHash = Animator.StringToHash("PlayerAnimationLayer.Airborne_Attack");
    }
    // Update is called once per frame
    void Update() 
    {
        AnimatorStateInfo animationStateInfo = playerMovement.playerAnimator.GetCurrentAnimatorStateInfo(0); //Pro zji�t�n� stavu animace, kter� zrovna hraje
        //Debug.Log("shootStateInfo: " + animationStateInfo.fullPathHash);
        //Debug.Log("normalAttackHash: " + animationStateInfo.fullPathHash);
        if (playerMovement.playerAnimator != null && !PlayerUIScript.GameIsPaused) 
        {

            /*mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);  //Zjist�, kde v t� main camera se nach�z� pozice my�i
            Vector3 aimingDirection = mousePos - playerMovement.playerRigidBody.transform.position;    //Po��t�, jak daleko je pozice my�i od hr��e
            float rotZ = Mathf.Atan2(aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg;*/

            //Debug.Log("normalAttackBlendController: " + normalAttackBlendController);
            playerMovement.playerAnimator.SetBool("isChargingShot", isChargingShot);
            playerMovement.playerAnimator.SetBool("isShooting", isShooting);
            playerMovement.playerAnimator.SetBool("canShootAirborne", canShootAirborne);

            if (Input.GetMouseButtonDown(0) && isShooting == false && playerMovement.isDashing == false)    //Kdy� za�ne st��let, tak se nem��e h�bat a za�ne natahovat luk
            {
                if (playerMovement.Grounded())
                {
                    isShooting = true;
                    isChargingShot = true;
                    playerMovement.canMove = false;
                    playerMovement.playerRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;    //Zmraz� pozici X i rotaci Z
                    playerMovement.playerAnimator.Play("Normal_Attack_Beginning", 0, 0f);   //Zahraje se animace Normal_Attack_Beginning
                    audioManager.PlaySFX(audioManager.bowString);
                }

                else if (!playerMovement.Grounded() && airShotsAvailable > 0 && canShootAirborne == true)   //Kdy� za�ne str�let a nen� na zemi, tak se mu povol� speci�ln� st��len� ve vzduchu
                {
                    shootAirborne();
                }
            }

            if (!playerMovement.Grounded() && airShotsAvailable == airShots){   //Kdy� nen� na zemi, m��e st��let ve vzduchu
                canShootAirborne = true;
                stopShooting();
            }

            else if (playerMovement.Grounded()) {   //Kdy� je na zemi, nem��e st��let ve vzduchu a airShotsAvailable se znova obnov� na svoj� origin�ln� hodnotu (airshots)
                airShotsAvailable = airShots;
                canShootAirborne = false;
            }

            if (playerMovement.isDashing && isShooting == true) {
                audioManager.StopSFX();
                stopShooting();
            }

            if (playerMovement.isJumping) {
                playerMovement.playerRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            }

            /*if (shootStateInfo.fullPathHash == -772934200) {   //Zjist�, jestli se se hash Animace shoduje se stavem t� animace -286001277 -1842485324 -772934200
                canShoot = true;
            }

            if (Input.GetMouseButtonUp(0) && canShoot == true && isChargingShot == true) {    //Vyst�el� ��p
                StartCoroutine(shootCooldown());
                SpawnArrow();
            }

            else if (Input.GetMouseButtonUp(0) && canShoot == false)    //Zru�� se mu nab�jen� ��pu, kdy� pust� my� p�ed nabit�m
            {
                StopCoroutine(shootCooldown());
                isShooting = false;
                isChargingShot = false;
                playerMovement.canMove = true;
            }*/

            if (Input.GetMouseButtonUp(0))
            {
                if (animationStateInfo.fullPathHash == chargingShotHash && isShooting == true && canShootAirborne == false && airShotsAvailable == 2) {    //Pokud stav animace, kter� zrovna hraje se bude rovna hashi animace, tak m��e vyst�elit Uncharged st�elu
                    StartCoroutine(shootUnchargedShot());
                }

                else if (animationStateInfo.fullPathHash == normalAttackHash)   //Pokud stav animace, kter� zrovna hraje se bude rovna hashi animace, tak m��e vyst�elit Charged st�elu
                {
                    shootChargedShot();
                }
            }

            if (!playSFXonce && animationStateInfo.fullPathHash == normalAttackHash)
            {
                audioManager.PlaySFX(audioManager.chargeShot);
                playSFXonce = true;
            }

            else if(animationStateInfo.fullPathHash != normalAttackHash) {
                playSFXonce = false;
            }

            /*if (aimingDirection.x < 0 )
            {
                // Kdy� m��� hr�� doleva, tak se mu oto�� zbra� doleva
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
                transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                // Kdy� m��� hr�� doprava, tak se mu oto�� zbra� doprava
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
                transform.localScale = new Vector3(1, 1, 1);
            }*/
        }

        void SpawnGravityArrow()   //Vytvo�� to ��p na pozici firePoint
        {
            playerStats.playerDamage = playerStats.playerInstantDamage;
            playerMovement.playerRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            spawnedArrow = Instantiate(Arrow, firePoint.transform.position, firePoint.transform.rotation);
            audioManager.StopSFX();
            audioManager.PlaySFX(audioManager.bowShoot); 

            ArrowScript arrowScriptComponent = spawnedArrow.GetComponent<ArrowScript>();

            IEnumerator changeArrowGravity()
            {
                Debug.Log("Changing gravity!");
                Rigidbody2D arrowRigidBody = arrowScriptComponent.arrowRigidBody;
                arrowRigidBody.gravityScale = 3f;

                if (arrowRigidBody != null)
                {
                    arrowRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    yield return new WaitForSeconds(0.1f);

                    if (arrowRigidBody != null) // Kontrola, zda byl Rigidbody2D zni�en v pr�b�hu �ek�n�
                    {
                        arrowRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                        Debug.Log("Gravity changed!");
                    }
                }
            }

                if (arrowScriptComponent != null)
            {
                StartCoroutine(changeArrowGravity());
            }
            else
            {
                Debug.LogError("ArrowScript component not found on the spawned arrow.");
            }
        }
        void ArrowDestroyedHandler()
        {
            // Odebr�n� skriptu ArrowScript ze ��pu
            ArrowScript arrowScriptComponent = spawnedArrow.GetComponent<ArrowScript>();
            if (arrowScriptComponent != null)
            {
                arrowScriptComponent.OnDestroyed -= ArrowDestroyedHandler; // Odstran�n� handleru ud�losti
                arrowScriptComponent.RemoveArrowScript(); // Odebr�n� skriptu
            }
        }

        void SpawnArrow()   //Vytvo�� to ��p na pozici firePoint
        {
            Instantiate(Arrow, firePoint.transform.position, firePoint.transform.rotation);
            audioManager.PlaySFX(audioManager.bowShoot);
        }


        void stopShooting() //Zastav� st��len� a m��e se znova pohybovat
        {
            isChargingShot = false;
            isShooting = false;
            playerMovement.canMove = true;
        }

        void shootChargedShot() {   //Vyst�el�, zastav� st��len� a odmrzne mu pozice X
            playerStats.playerDamage = playerStats.playerChargeDamage;
            stopShooting();
            SpawnArrow();
            playerMovement.playerRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }

        void shootAirborne() {  //Zahraje se animace, vyst�el�, zmen�� se mu o jedno airShotsAvailable a startne funkci shootAirborneCooldown()
            playerStats.playerDamage = playerStats.playerInstantDamage;
            playerMovement.playerAnimator.Play("Airborne_Attack", 0, 0.15f);
            isShooting = true;
            SpawnGravityArrow();
            StartCoroutine(shootAirborneCooldown());
            Debug.Log("AirShot");
            airShotsAvailable--;
        }
        
        IEnumerator shootAirborneCooldown() {   //Zmen�� se mu gravitace, stopne se na chvilku, nem��e se pohybovat a po�k� chvilku ne� se mu zm�n� gravitace do norm�lu a bude moct st��let
                playerMovement.playerRigidBody.gravityScale = 0.4f;
                playerMovement.playerRigidBody.velocity = Vector2.zero;
                playerMovement.canMove = false;
                playerMovement.playerRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                yield return new WaitForSeconds(0.25f);
                playerMovement.playerRigidBody.gravityScale = 1f;
                isShooting = false;
                playerMovement.canMove = true;
                Debug.Log("Cooldown passed!"); 
        }

        IEnumerator shootUnchargedShot() {  //Zahraje animaci, vyst�el�, nem��e se pohybovat a po�ka chvilku ne� se bude moct pohybovat o odmrzne mu pozice X
            playerMovement.playerAnimator.Play("Instant_Attack", 0, 0.1f);
            SpawnGravityArrow();
            playerMovement.canMove = false;
            yield return new WaitForSeconds(0.2f);
            stopShooting();
            playerMovement.playerRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
