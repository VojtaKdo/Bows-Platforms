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
        normalAttackHash = Animator.StringToHash("PlayerAnimationLayer.Normal_Attack_Loop"); //Pøevede to animaci Normal_Attack_Loop do unikátního hashe 
        chargingShotHash = Animator.StringToHash("PlayerAnimationLayer.Normal_Attack_Beginning");
        airborneAttackHash = Animator.StringToHash("PlayerAnimationLayer.Airborne_Attack");
    }
    // Update is called once per frame
    void Update() 
    {
        AnimatorStateInfo animationStateInfo = playerMovement.playerAnimator.GetCurrentAnimatorStateInfo(0); //Pro zjištìní stavu animace, která zrovna hraje
        //Debug.Log("shootStateInfo: " + animationStateInfo.fullPathHash);
        //Debug.Log("normalAttackHash: " + animationStateInfo.fullPathHash);
        if (playerMovement.playerAnimator != null && !PlayerUIScript.GameIsPaused) 
        {

            /*mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);  //Zjistí, kde v té main camera se nachází pozice myši
            Vector3 aimingDirection = mousePos - playerMovement.playerRigidBody.transform.position;    //Poèítá, jak daleko je pozice myši od hráèe
            float rotZ = Mathf.Atan2(aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg;*/

            //Debug.Log("normalAttackBlendController: " + normalAttackBlendController);
            playerMovement.playerAnimator.SetBool("isChargingShot", isChargingShot);
            playerMovement.playerAnimator.SetBool("isShooting", isShooting);
            playerMovement.playerAnimator.SetBool("canShootAirborne", canShootAirborne);

            if (Input.GetMouseButtonDown(0) && isShooting == false && playerMovement.isDashing == false)    //Když zaène støílet, tak se nemùže hýbat a zaène natahovat luk
            {
                if (playerMovement.Grounded())
                {
                    isShooting = true;
                    isChargingShot = true;
                    playerMovement.canMove = false;
                    playerMovement.playerRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;    //Zmrazí pozici X i rotaci Z
                    playerMovement.playerAnimator.Play("Normal_Attack_Beginning", 0, 0f);   //Zahraje se animace Normal_Attack_Beginning
                    audioManager.PlaySFX(audioManager.bowString);
                }

                else if (!playerMovement.Grounded() && airShotsAvailable > 0 && canShootAirborne == true)   //Když zaène strílet a není na zemi, tak se mu povolí speciální støílení ve vzduchu
                {
                    shootAirborne();
                }
            }

            if (!playerMovement.Grounded() && airShotsAvailable == airShots){   //Když není na zemi, mùže støílet ve vzduchu
                canShootAirborne = true;
                stopShooting();
            }

            else if (playerMovement.Grounded()) {   //Když je na zemi, nemùže støílet ve vzduchu a airShotsAvailable se znova obnoví na svojí originální hodnotu (airshots)
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

            /*if (shootStateInfo.fullPathHash == -772934200) {   //Zjistí, jestli se se hash Animace shoduje se stavem té animace -286001277 -1842485324 -772934200
                canShoot = true;
            }

            if (Input.GetMouseButtonUp(0) && canShoot == true && isChargingShot == true) {    //Vystøelí šíp
                StartCoroutine(shootCooldown());
                SpawnArrow();
            }

            else if (Input.GetMouseButtonUp(0) && canShoot == false)    //Zruší se mu nabíjení šípu, když pustí myš pøed nabitím
            {
                StopCoroutine(shootCooldown());
                isShooting = false;
                isChargingShot = false;
                playerMovement.canMove = true;
            }*/

            if (Input.GetMouseButtonUp(0))
            {
                if (animationStateInfo.fullPathHash == chargingShotHash && isShooting == true && canShootAirborne == false && airShotsAvailable == 2) {    //Pokud stav animace, která zrovna hraje se bude rovna hashi animace, tak mùže vystøelit Uncharged støelu
                    StartCoroutine(shootUnchargedShot());
                }

                else if (animationStateInfo.fullPathHash == normalAttackHash)   //Pokud stav animace, která zrovna hraje se bude rovna hashi animace, tak mùže vystøelit Charged støelu
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
                // Když míøí hráè doleva, tak se mu otoèí zbraò doleva
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
                transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                // Když míøí hráè doprava, tak se mu otoèí zbraò doprava
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
                transform.localScale = new Vector3(1, 1, 1);
            }*/
        }

        void SpawnGravityArrow()   //Vytvoøí to šíp na pozici firePoint
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

                    if (arrowRigidBody != null) // Kontrola, zda byl Rigidbody2D znièen v prùbìhu èekání
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
            // Odebrání skriptu ArrowScript ze šípu
            ArrowScript arrowScriptComponent = spawnedArrow.GetComponent<ArrowScript>();
            if (arrowScriptComponent != null)
            {
                arrowScriptComponent.OnDestroyed -= ArrowDestroyedHandler; // Odstranìní handleru události
                arrowScriptComponent.RemoveArrowScript(); // Odebrání skriptu
            }
        }

        void SpawnArrow()   //Vytvoøí to šíp na pozici firePoint
        {
            Instantiate(Arrow, firePoint.transform.position, firePoint.transform.rotation);
            audioManager.PlaySFX(audioManager.bowShoot);
        }


        void stopShooting() //Zastaví støílení a mùže se znova pohybovat
        {
            isChargingShot = false;
            isShooting = false;
            playerMovement.canMove = true;
        }

        void shootChargedShot() {   //Vystøelí, zastaví støílení a odmrzne mu pozice X
            playerStats.playerDamage = playerStats.playerChargeDamage;
            stopShooting();
            SpawnArrow();
            playerMovement.playerRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }

        void shootAirborne() {  //Zahraje se animace, vystøelí, zmenší se mu o jedno airShotsAvailable a startne funkci shootAirborneCooldown()
            playerStats.playerDamage = playerStats.playerInstantDamage;
            playerMovement.playerAnimator.Play("Airborne_Attack", 0, 0.15f);
            isShooting = true;
            SpawnGravityArrow();
            StartCoroutine(shootAirborneCooldown());
            Debug.Log("AirShot");
            airShotsAvailable--;
        }
        
        IEnumerator shootAirborneCooldown() {   //Zmenší se mu gravitace, stopne se na chvilku, nemùže se pohybovat a poèká chvilku než se mu zmìní gravitace do normálu a bude moct støílet
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

        IEnumerator shootUnchargedShot() {  //Zahraje animaci, vystøelí, nemùže se pohybovat a poèka chvilku než se bude moct pohybovat o odmrzne mu pozice X
            playerMovement.playerAnimator.Play("Instant_Attack", 0, 0.1f);
            SpawnGravityArrow();
            playerMovement.canMove = false;
            yield return new WaitForSeconds(0.2f);
            stopShooting();
            playerMovement.playerRigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
