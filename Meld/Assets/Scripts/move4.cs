using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move4 : MonoBehaviour
{
    public float speed = 1;
    public float topSpeed = 2;
    public float maxSeparation = 2;
    public float jump = 10;
    private Rigidbody rb;
    Vector3 movement;
    private GameObject player1;
    private GameObject player2;
    private GameObject membrane;
    private GameObject membraneSupport;

    public string inputHorizontal;
    public string inputVertical;
    public string player1Tag = "Player1";
    public string player2Tag = "Player2";
    public string membraneTag = "Membrane";
    public string membraneSupportSphereTag = "MembraneSupportSphere";

    public float jumpMagnitude = 20f;
    public float raycastDistance = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        player1 = GameObject.FindWithTag(player1Tag);
        player2 = GameObject.FindWithTag(player2Tag);
        membrane = GameObject.FindWithTag(membraneTag);
        membraneSupport = GameObject.FindWithTag(membraneSupportSphereTag);
        Physics.IgnoreCollision(player1.GetComponent<Collider>(), membraneSupport.GetComponent<Collider>());
        Physics.IgnoreCollision(player2.GetComponent<Collider>(), membraneSupport.GetComponent<Collider>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (true || (Vector3.Distance(player1.transform.position, player2.transform.position) <= maxSeparation))
        {

            float horizontal = Input.GetAxis(inputHorizontal) * 20;
            float vertical = Input.GetAxis(inputVertical) * 20;

            Vector3 movement = new Vector3(horizontal, 0.0f, vertical);

            //rb.AddForce(movement * speed);


            // Controlling movement speed on the XZ plane
            movement = movement.normalized * speed;
            if (movement.magnitude > topSpeed)
                movement = movement.normalized * topSpeed;
            // This is to preserve Y movement so that gravity affects it properly
            movement.y = rb.velocity.y;

            rb.velocity = movement;
        }

        // Checking if we are above the ground, stop moving so we don't sink out of the blob
        var ray = new Ray(transform.position, Vector3.down);
        bool grounded = Physics.Raycast(ray, raycastDistance);

        var player1Ray = new Ray(player1.transform.position, Vector3.down);
        bool player1grounded = Physics.Raycast(player1Ray, raycastDistance);
        var player2Ray = new Ray(player2.transform.position, Vector3.down);
        bool player2grounded = Physics.Raycast(player2Ray, raycastDistance);
        grounded = player1grounded || player2grounded;

        // This should prevent the player from sinking to the ground
        rb.useGravity = !grounded;
        // This should extra prevent the player from sinking
        if (grounded)
        {
            var minVelocity = rb.velocity;
            minVelocity.y = Math.Max(0, minVelocity.y);
            rb.velocity = minVelocity;
        }

        Vector3 player1Position = player1.transform.position;
        Vector3 player2Position = player2.transform.position;
        player1Position.y = 0;
        player2Position.y = 0;

        float playerDistance = Vector3.Distance(player1Position, player2Position);
        Vector3 avg = (player1.transform.position + player2.transform.position) / 2;

        // start shaking the membrane by pulling players together gently when they're close to max separation
        if ((maxSeparation - 1.5) <= playerDistance && playerDistance < maxSeparation)
        {
            jumpMagnitude = 10;
            if ((maxSeparation - 0.5) <= playerDistance && playerDistance < maxSeparation)
            {
                jumpMagnitude = 15;
            }

            player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * jumpMagnitude);
            player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * jumpMagnitude);
            membrane.transform.position = avg;
            membraneSupport.transform.position = avg;
        }
        if (playerDistance > maxSeparation || !grounded)
        {

            if (grounded)
            {
                avg.y = jump;
            }

            jumpMagnitude = 150;
            player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * jumpMagnitude);
            player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * jumpMagnitude);

            Vector3 membraneAvg = avg + 0.25f * Vector3.up;
            //membrane.transform.localScale = new Vector3(1.75f, 1.25f, 1.75f);
            membrane.transform.position = membraneAvg;
            float xDistance = Math.Abs(player1.transform.position.x - player2.transform.position.x);
            float yDistance = Math.Abs(player1.transform.position.y - player2.transform.position.y);
            float zDistance = Math.Abs(player1.transform.position.z - player2.transform.position.z);
            membraneSupport.transform.localScale = new Vector3(0.75f * playerDistance, 0.75f * playerDistance, 0.75f * playerDistance);
            membraneSupport.transform.position = avg + 0.25f * Vector3.down;


        }
    }
}
