using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerBehaviour : MonoBehaviour
{
    private float speed = GameManager.speed;
    private float topSpeed = GameManager.topSpeed;
    private bool isGrounded;
    private bool isSticking = false;
    private RaycastHit playerGroundedHit;
    private GameObject player;
    private Rigidbody rb;
    private float raycastDistance = GameManager.rayCastDistance;

    public RaycastHit GetGroundedHit()
    {
        return playerGroundedHit;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    public bool GetIsSticking()
    {
        return isSticking;
    }

    public void SetIsSticking(bool isPlayerSticking)
    {
        isSticking = isPlayerSticking;
    }

    public void SetPlayerMass(bool isPartnerSticking)
    {
        if (isPartnerSticking)
        {
            rb.mass = GameManager.stickingPlayerMass;
        }
        else
        {
            rb.mass = GameManager.playerMass;
        }
    }

    public void SetVelocity(float horizontalAxis, float verticalAxis, bool isPartnerSticking)
    {
        // Rotates the input vector to match the cameras Y rotation
        temp = alignVectorToCurrentCamera(new Vector3(horizontalAxis,0f,verticalAxis));
        horizontalAxis = temp.x;
        verticalAxis = temp.z;

        var temp = rb.velocity;

        if (!isPartnerSticking) {
            if (horizontalAxis == 0)
                temp.x -= (rb.velocity.x) * Time.deltaTime;
            if (verticalAxis == 0)
                temp.z -= (rb.velocity.z) * Time.deltaTime;
        }

        rb.velocity = temp;

        Vector3 movement = new Vector3(horizontalAxis, 0.0f, verticalAxis);
        SetPlayerMass(isPartnerSticking);
 
        movement = movement.normalized * speed;
        if (movement.magnitude > topSpeed)
            movement = movement.normalized * topSpeed;
        
        movement.y = 0;

        if (!isPartnerSticking){
            if ((rb.velocity + movement).magnitude < rb.velocity.magnitude) {
                movement.x = movement.x * 1.2f;
                movement.z = movement.z * 1.2f;
            }
        }

        if (!isPartnerSticking) {
            if ((rb.velocity + movement).magnitude < rb.velocity.magnitude) {
                movement.x = movement.x * 1.5f;
                movement.z = movement.z * 1.5f;
            }
        }

        if (rb.velocity.magnitude <= (topSpeed/2)) {
            rb.velocity +=  movement;
        } else {
            rb.AddForce(movement);
        }
        
        Vector3 clampVel = rb.velocity;
        if (isPartnerSticking)
        {
            clampVel.x = Mathf.Clamp(clampVel.x, -topSpeed *20, topSpeed * 20);
            clampVel.z = Mathf.Clamp(clampVel.z, -topSpeed *20, topSpeed *20);
        } else
        {
            // normal movement on ground  
            clampVel.x = Mathf.Clamp(clampVel.x, -topSpeed, topSpeed);
            clampVel.z = Mathf.Clamp(clampVel.z, -topSpeed, topSpeed);
        }
        rb.velocity = clampVel;


        // Use this block if you want simple movement without momentum
        // This is to preserve Y movement so that gravity affects it properly
        //movement.y = rb.velocity.y;
        //rb.velocity = movement;

        if ( movement.x != 0 ||  movement.z != 0) {
            player.transform.Find("Canvas").gameObject.transform.Find("Arrow").gameObject.GetComponent<Image>().rectTransform.localRotation =  Quaternion.Euler(0, 0, (Mathf.Atan2(movement.z, movement.x) * Mathf.Rad2Deg + 180));
        }
    }

    public Vector3 alignVectorToCurrentCamera(Vector3 movement) {
        // This removes the downward rotation so we only have the direction the camera 
        // is facing on the X-Z plane which is what our movement input cares about
        var camdir = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 directedMovement = Quaternion.Euler(camdir) * movement; //Camera.main.transform.TransformDirection(movement);
        directedMovement.y = 0;
        directedMovement = directedMovement.normalized * movement.magnitude;
        return directedMovement;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        rb = player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var ray = new Ray(player.transform.position, Vector3.down);
        isGrounded = Physics.SphereCast(ray, player.GetComponent<SphereCollider>().radius, out playerGroundedHit, raycastDistance);
    }
}
