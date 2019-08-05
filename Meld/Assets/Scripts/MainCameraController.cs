using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCameraController : MonoBehaviour
{
    // Copied code from Survival Shooter Tutorial
    
    public moveCleanUnityCloth2 playerBlob;
    public Transform player1;
    public Transform player2;           
    private float smoothing = 10f;        // The speed with which the camera will be following.
    private float rotationSmoothing = 60f;        // The speed with which the camera will rotate for new perspectives

    Vector3 defaultOffset;                     // The initial offset from the target.
    Quaternion defaultRotation;
    Vector3 topdownOffset;
    Vector3 mazeOffset;
    Quaternion topdownRotation;
    Quaternion mazeRotation;

    Vector3 currentOffset;

    private float rotatedDegrees = 0;
    public bool flipRotation = false;
    private bool flipped = false;

    void Start ()
    {
        var player = GameObject.FindWithTag("Player");
        player1 = GameObject.FindWithTag("Player1").transform;
        player2 = GameObject.FindWithTag("Player2").transform;
        // Calculate the initial offset.
        defaultOffset = transform.position - getTargetPos();
        defaultOffset.x = 0;
        defaultRotation = transform.rotation;
        currentOffset = defaultOffset;


        topdownOffset = GameObject.FindWithTag("mazecam").transform.position - getTargetPos();
        //mazeOffset = GameObject.FindWithTag("mazecam").transform.position - getTargetPos();
        // This centers the camera above the player
        topdownOffset.x = 0;
        topdownOffset.z = 0;
        // topdownOffset.y = 13;
        mazeOffset = topdownOffset;
        topdownOffset.y = mazeOffset.y * 2 / 3;
        topdownRotation = GameObject.FindWithTag("mazecam").transform.rotation;
        mazeRotation = topdownRotation;

        playerBlob = player.GetComponent<moveCleanUnityCloth2>();
    }

    void FixedUpdate ()
    {
        
        // Camera mazecam = GameObject.FindWithTag("mazecam").GetComponent<Camera>();
        // Camera maincam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //Camera revcam = GameObject.FindWithTag("ReverseCamera").GetComponent<Camera>();
        
        if (!flipRotation){
            if (playerBlob.p1Maze || playerBlob.p2Maze)
            {
                if (currentOffset != mazeOffset)
                    currentOffset = Vector3.Lerp (currentOffset, mazeOffset, smoothing * Time.deltaTime);
                if (!qEquals(transform.rotation, mazeRotation))
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, mazeRotation, rotationSmoothing*Time.deltaTime);
            }
            else if (!canSeePlayers()){
                if (currentOffset != topdownOffset)
                    currentOffset = Vector3.Lerp (currentOffset, topdownOffset, smoothing * Time.deltaTime);
                if (!qEquals(transform.rotation, topdownRotation))
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, topdownRotation, rotationSmoothing*Time.deltaTime);
            }
            else
            {
                if (currentOffset != defaultOffset)
                    currentOffset = Vector3.Lerp (currentOffset, defaultOffset, smoothing * Time.deltaTime);
                if (!qEquals(transform.rotation, defaultRotation))
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, rotationSmoothing*Time.deltaTime);
                // else if (flipRotation) {
                //     transform.RotateAround (getTargetPos(), Vector3.up, 180);
                //     defaultOffset = transform.position - getTargetPos();
                //     defaultRotation = transform.rotation;
                //     flipRotation = false;
                //     flipped = !flipped;
                //     topdownRotation *= GetNormalized(Quaternion.AngleAxis(180, new Vector3(0,0,1)));//(new Vector3(0,0,180));
                // }
            }
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = getTargetPos() + currentOffset;
            
            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
        else {
            if (rotatedDegrees != 180) {
                var dtr = 20 * Time.deltaTime * smoothing;
                dtr = (rotatedDegrees + dtr) > 180 ? 180 - rotatedDegrees : dtr;
                transform.RotateAround (getTargetPos(),Vector3.up, dtr);
                rotatedDegrees = Math.Min(180, rotatedDegrees + dtr);
            }
            else {
                rotatedDegrees = 0;
                defaultOffset.z *= -1;
                defaultRotation *= GetNormalized(Quaternion.AngleAxis(180, new Vector3(0,1,0)));
                defaultRotation *= GetNormalized(Quaternion.AngleAxis(90, new Vector3(1,0,0)));
                currentOffset = transform.position - getTargetPos();
                flipRotation = false;
                flipped = !flipped;
                topdownRotation *= GetNormalized(Quaternion.AngleAxis(180, new Vector3(0,0,1)));
                // // Set the current position to be the default one
                // defaultOffset = transform.position - getTargetPos();
                // currentOffset = defaultOffset;
                // defaultRotation = transform.rotation;
                // flipRotation = false;
            }
        }
    }

    public void flipCamera(){
        flipRotation = true;
    }

    Vector3 getTargetPos (){
        var targetPos = (player1.position + player2.position) / 2;
        targetPos.y = Math.Max(player1.position.y, player2.position.y);
        return targetPos;
    }
    Quaternion GetNormalized(Quaternion q){
        float f = 1f / Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        return new Quaternion(q.x*f, q.y*f, q.z*f, q.w*f);
    }

    bool qEquals(Quaternion q1, Quaternion q2){
        return (q1.Equals(q2)) || (q1 == q2);
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
