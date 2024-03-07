using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class knightSkeletonStats : MonoBehaviour
{
    public float knightSkeletonHP = 10;
    public float knightSkeletonMovementSpeed = 3;
    public float knightSkeletonMaxHP = 10;
    public float knightSkeletonDamage = 2;
    public bool isKnightSkeletonAttacking;
    public bool agroOnce;
    public bool isKnightSkeletonWalking;

    void Update()
    {
        if (knightSkeletonHP <= 0) { 
            Destroy(gameObject);
        }
    }
}
