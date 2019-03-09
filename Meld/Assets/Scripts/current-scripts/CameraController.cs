using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    // Copied code from Survival Shooter Tutorial
    
    public moveCleanUnityCloth2 playerBlob;
    public Transform player1;
    public Transform player2;           
    public float smoothing = 5f;        // The speed with which the camera will be following.
    public float rotationSmoothing = 40f;        // The speed with which the camera will rotate for new perspectives

    Vector3 defaultOffset;                     // The initial offset from the target.
    Quaternion defaultRotation;
    Vector3 topdownOffset;
    Quaternion topdownRotation;

    Vector3 currentOffset;

    void Start ()
    {
        var player = GameObject.FindWithTag("Player");
        // Calculate the initial offset.
        defaultOffset = transform.position - getTargetPos();
        defaultRotation = transform.rotation;
        currentOffset = defaultOffset;
        topdownOffset = GameObject.FindWithTag("mazecam").transform.position - getTargetPos();
        // This centers the camera above the player
        topdownOffset.x = 0;
        topdownOffset.z = 0;
        topdownRotation = GameObject.FindWithTag("mazecam").transform.rotation;
        playerBlob = player.GetComponent<moveCleanUnityCloth2>();
    }

    void FixedUpdate ()
    {
        
        // Camera mazecam = GameObject.FindWithTag("mazecam").GetComponent<Camera>();
        // Camera maincam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //Camera revcam = GameObject.FindWithTag("ReverseCamera").GetComponent<Camera>();

        if (playerBlob.p1Maze || playerBlob.p2Maze)
        {
            if (currentOffset != topdownOffset)
                currentOffset = Vector3.Lerp (currentOffset, topdownOffset, smoothing * Time.deltaTime);
            if (transform.rotation != topdownRotation)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, topdownRotation, rotationSmoothing*Time.deltaTime);
            // maincam.enabled = false;
            // mazecam.enabled = true;
            // mazecam.GetComponent<CameraController>().enabled = true;
        }
        else
        {
            if (currentOffset != defaultOffset)
                currentOffset = Vector3.Lerp (currentOffset, defaultOffset, smoothing * Time.deltaTime);
            if (transform.rotation != defaultRotation)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, rotationSmoothing*Time.deltaTime);
            // maincam.enabled = true;
            // mazecam.enabled = false;
            // mazecam.GetComponent<CameraController>().enabled = false;
        }


        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = getTargetPos() + currentOffset;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    Vector3 getTargetPos (){
        var targetPos = (player1.position + player2.position) / 2;
        targetPos.y = Math.Max(player1.position.y, player2.position.y);
        return targetPos;

    }
}
