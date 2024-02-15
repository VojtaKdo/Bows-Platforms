using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D playerRigidBody;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Camera
        Camera playerCamera = Camera.main;
        playerCamera.transform.position = new Vector3(playerRigidBody.position.x, playerRigidBody.position.y, -10); //Pozice kamery
    }
}
