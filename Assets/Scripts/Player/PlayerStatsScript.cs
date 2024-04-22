using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatsScript : MonoBehaviour
{
    HealthBarScript healthBar;
    public static float playerHP = 10;
    public float previousHealth;
    public static float playerMaxHP = 10;
    public float playerDamage;
    public float playerInstantDamage = 1;
    public float playerChargeDamage = 2;
    public float playerMovementSpeed = 5;
    public float playerJumpPower = 8;
    public float playerDashPower = 20;
    public float playerDashingTime = 2;
    public float playerDashingCooldown = 0.2f;
    public float playerNumberOfDashes = 1;
    public float playerDashesAvailable = 1;
    public static bool isPlayerInvincible;

    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("PlayerUI").GetComponentInChildren<HealthBarScript>();
        previousHealth = playerHP;
    }

    //Každých 0,5 vteøin se mu dá nesmrtelnost
    public static IEnumerator PlayerInvicibility() {
        Debug.Log("Player is invincible"); 
        isPlayerInvincible = true;
        yield return new WaitForSeconds(0.5f); 
        isPlayerInvincible = false;
        Debug.Log("Player is not invincible");
    }

    void Update()
    {
        //Koukne, jestli se høáèovi mìní životy
        float currentHealth = playerHP;

        //Když hráè umøe, tak se respawne v tom stejným levelu
        if (playerHP <= 0) {    //trolled
            Debug.Log("Player left");
            SceneManager.LoadScene(0);
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        }

        //Pokud se zmìní hráèovi životy, tak se mu dá nesmrtelnost a aktualizuje se mu hpBar
        if (currentHealth != previousHealth) {
            Debug.Log("Player is taking damage!");
            healthBar.UpdateHealthBarImage(playerHP, playerMaxHP);
            StartCoroutine(PlayerInvicibility());
        } 

        previousHealth = currentHealth;
    }
}
