using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move3 : MonoBehaviour
{
    public float speed = 1;
    public float topSpeed = 2;
    public float maxSeperation = 2;
    public float jump = 10;
    private Rigidbody cube;
    Vector3 movement;
    private GameObject player1;
    private GameObject player2;
    private GameObject membrane;

    public string inputHorizontal;
    public string inputVertical;
    public string player1Tag = "Player1";
    public string player2Tag = "Player2";
    public string membraneTag = "Membrane";

    public float jumpMagnitude = 20f;
    public float raycastDistance = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        cube = GetComponent<Rigidbody>();
        cube.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        player1 = GameObject.FindWithTag(player1Tag);
        player2 = GameObject.FindWithTag(player2Tag);
        membrane = GameObject.FindWithTag(membraneTag);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (true || (Vector3.Distance(player1.transform.position, player2.transform.position) <= maxSeperation))
        {

            float horizontal = Input.GetAxis(inputHorizontal) * 20;
            float vertical = Input.GetAxis(inputVertical) * 20;

            Vector3 movement = new Vector3(horizontal, 0.0f, vertical);

            //cube.AddForce(movement * speed);


            // Controlling movement speed on the XZ plane
            movement = movement.normalized * speed;
            if (movement.magnitude > topSpeed)
                movement = movement.normalized * topSpeed;
            // This is to preserve Y movement so that gravity affects it properly
            movement.y = cube.velocity.y;

            cube.velocity = movement;

            //cube.AddForce(movement);
            //Vector3 vel = cube.velocity;
            //vel.x = horizontal;
            //vel.y = vertical;
            //        cube.velocity = vel;
        }

        // Checking if we are above the ground, stop moving so we don't sink out of the blob
        var ray = new Ray(transform.position, Vector3.down);
        bool grounded = Physics.Raycast(ray, raycastDistance);
        
        var rAy = new Ray(player1.transform.position, Vector3.down);
        bool player1grounded = Physics.Raycast(rAy, raycastDistance);
        var raY = new Ray(player2.transform.position, Vector3.down);
        bool player2grounded = Physics.Raycast(raY, raycastDistance);
        grounded = player1grounded || player2grounded;
        
        // This should prevent the player from sinking to the ground
        cube.useGravity = !grounded;
        // This should extra prevent the player from sinking
        if (grounded)
        {
            var temp = cube.velocity;
            temp.y = Math.Max(0, temp.y);
            cube.velocity = temp;
        }
        
        Vector3 player1position = player1.transform.position;
        Vector3 player2position = player2.transform.position;
        player1position.y = 0;
        player2position.y = 0;

        if (Vector3.Distance(player1position, player2position) > maxSeperation || !grounded)
        {
            Vector3 avg = (player1.transform.position + player2.transform.position) / 2;
            //player1.transform.position = avg;
            //player2.transform.position = avg;
            //membrane.transform.position = avg;
            
            if (grounded)
                avg.y = jump;

            player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * jumpMagnitude);
            player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * jumpMagnitude);

            //player1.transform.position = avg;
            //player2.transform.position = avg;
            
            //membrane.transform.position = avg;
        }
    }
}
