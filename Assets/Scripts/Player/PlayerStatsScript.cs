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

    //Ka�d�ch 0,5 vte�in se mu d� nesmrtelnost
    public static IEnumerator PlayerInvicibility() {
        Debug.Log("Player is invincible"); 
        isPlayerInvincible = true;
        yield return new WaitForSeconds(0.5f); 
        isPlayerInvincible = false;
        Debug.Log("Player is not invincible");
    }

    void Update()
    {
        //Koukne, jestli se h���ovi m�n� �ivoty
        float currentHealth = playerHP;

        //Kdy� hr�� um�e, tak se respawne v tom stejn�m levelu
        if (playerHP <= 0) {    //trolled
            Debug.Log("Player left");
            SceneManager.LoadScene(0);
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        }

        //Pokud se zm�n� hr��ovi �ivoty, tak se mu d� nesmrtelnost a aktualizuje se mu hpBar
        if (currentHealth != previousHealth) {
            Debug.Log("Player is taking damage!");
            healthBar.UpdateHealthBarImage(playerHP, playerMaxHP);
            StartCoroutine(PlayerInvicibility());
        } 

        previousHealth = currentHealth;
    }
}
