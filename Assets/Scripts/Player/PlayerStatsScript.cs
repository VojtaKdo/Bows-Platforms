using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsScript : MonoBehaviour
{
    public float playerHP = 10;
    public float playerMaxHP = 10;
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
    public bool isPlayerInvincible;

    public IEnumerator PlayerInvicibility() {
        Debug.Log("Player is invincible"); 
        isPlayerInvincible = true;
        yield return new WaitForSeconds(0.5f);
        isPlayerInvincible = false;
    }
}
