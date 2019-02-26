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

    public void SetVelocity(float horizontalAxis, float verticalAxis)
    {
        float horizontal = horizontalAxis * 20;
        float vertical = verticalAxis * 20;

        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
 
        movement = movement.normalized * speed;
        if (movement.magnitude > topSpeed)
            movement = movement.normalized * topSpeed;
        // This is to preserve Y movement so that gravity affects it properly
        movement.y = rb.velocity.y;

        rb.velocity = movement;
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
