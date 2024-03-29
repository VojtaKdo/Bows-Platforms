using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D playerRigidBody;
    public Camera playerCamera;
    public float tutorialProgress = 1;

    void Start()
    {
        playerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Camera
        playerCamera.transform.position = new Vector3(playerRigidBody.position.x, playerRigidBody.position.y, -10); //Pozice kamery
    }
}
