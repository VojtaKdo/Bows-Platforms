using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D playerRigidBody;
    public Camera playerCamera;
    public static float smoothTime = 0.1f;
    public static float yOffSet = 0;
    public Vector3 velocity = Vector3.zero;

    void Start()
    {
        playerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Camera
        if (playerRigidBody.transform.position.y >= 0)
        {
            playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, new Vector3(playerRigidBody.position.x, playerRigidBody.position.y, -10), ref velocity, smoothTime);
        }
        else {
            playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, new Vector3(playerRigidBody.position.x, 0, -10), ref velocity, smoothTime);
        }
    }
}
