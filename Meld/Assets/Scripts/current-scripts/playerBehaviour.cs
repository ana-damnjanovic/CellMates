using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 movement = new Vector3(horizontalAxis, 0.0f, verticalAxis);
        SetPlayerMass(isPartnerSticking);
 
        movement = movement.normalized * speed;
        if (movement.magnitude > topSpeed)
            movement = movement.normalized * topSpeed;

        movement.y = 0;
        rb.AddForce(movement);
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
