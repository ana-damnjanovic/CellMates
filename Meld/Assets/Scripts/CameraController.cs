using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Copied code from Survival Shooter Tutorial
    
    public Transform player1;
    public Transform player2;           
    public float smoothing = 5f;        // The speed with which the camera will be following.

    Vector3 offset;                     // The initial offset from the target.


    void Start ()
    {
        // Calculate the initial offset.
        offset = transform.position - getTargetPos();
    }

    void FixedUpdate ()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = getTargetPos() + offset;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    Vector3 getTargetPos (){
        return (player1.position + player2.position) / 2;
    }
}
