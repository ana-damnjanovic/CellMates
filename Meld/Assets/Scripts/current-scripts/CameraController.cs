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
    Vector3 mazeOffset;
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
        //mazeOffset = GameObject.FindWithTag("mazecam").transform.position - getTargetPos();
        // This centers the camera above the player
        topdownOffset.x = 0;
        topdownOffset.z = 0;
        mazeOffset = topdownOffset;
        topdownOffset.y = mazeOffset.y * 2 / 3;
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
            if (currentOffset != mazeOffset)
                currentOffset = Vector3.Lerp (currentOffset, mazeOffset, smoothing * Time.deltaTime);
            if (transform.rotation != topdownRotation)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, topdownRotation, rotationSmoothing*Time.deltaTime);
            // maincam.enabled = false;
            // mazecam.enabled = true;
            // mazecam.GetComponent<CameraController>().enabled = true;
        }
        else if (!canSeePlayers()){
            if (currentOffset != topdownOffset)
                currentOffset = Vector3.Lerp (currentOffset, topdownOffset, smoothing * Time.deltaTime);
            if (transform.rotation != topdownRotation)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, topdownRotation, rotationSmoothing*Time.deltaTime);
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

    bool canSeePlayers(){
        // Makes sticking rays ignore players, cell membrane, and support structure
        // LayerMask playerLayer = 1 << 9;
        //LayerMask cellLayer = 1 << 10;
        //LayerMask supportLayer = 1 << 11;
        //LayerMask layerMask = ~(cellLayer | supportLayer);

        Vector3 defaultPos = getTargetPos() + defaultOffset;

        RaycastHit rayHit;
        RaycastHit hitray1;
        RaycastHit hitray2;
        bool hit1 = Physics.Raycast(defaultPos, player1.position - defaultPos, out hitray1, Mathf.Infinity);
        bool hit2 = Physics.Raycast(defaultPos, player2.position - defaultPos, out hitray2, Mathf.Infinity);
        hit1 = (hit1 && (hitray1.transform.CompareTag("Player1") || hitray1.transform.CompareTag("Player2")));
        hit2 = (hit2 && (hitray2.transform.CompareTag("Player1") || hitray2.transform.CompareTag("Player2")));
        Debug.DrawRay(defaultPos, player1.position - defaultPos, hit1 ? Color.green : Color.red);
        Debug.DrawRay(defaultPos, player2.position - defaultPos, hit2 ? Color.green : Color.red);
        return hit1 || hit2;
    }
}
