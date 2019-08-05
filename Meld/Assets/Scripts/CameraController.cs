using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This controller will make the object follow the player around. It will stay where it is relative to the player.
public class CameraController : MonoBehaviour
{
    public moveCleanUnityCloth2 playerBlob;
    public Transform player1;
    public Transform player2;           
    public float smoothing = 5f;        // The speed with which the camera will be following.
    public float rotationSmoothing = 40f;        // The speed with which the camera will rotate for new perspectives

    Vector3 defaultOffset;                     // The initial offset from the target.
    Quaternion defaultRotation;

    Vector3 currentOffset;

    void Start ()
    {
        var player = GameObject.FindWithTag("Player");
        player1 = GameObject.FindWithTag("Player1").transform;
        player2 = GameObject.FindWithTag("Player2").transform;
        // Calculate the initial offset.
        defaultOffset = transform.position - getTargetPos();
        defaultRotation = transform.rotation;
        currentOffset = defaultOffset;

        playerBlob = player.GetComponent<moveCleanUnityCloth2>();
    }

    void FixedUpdate ()
    {
        // If the current position/rotation relative to the players is not the default
        // transition to the default
        if (currentOffset != defaultOffset)
            currentOffset = Vector3.Lerp (currentOffset, defaultOffset, smoothing * Time.deltaTime);
        if (transform.rotation != defaultRotation)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, rotationSmoothing*Time.deltaTime);

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
