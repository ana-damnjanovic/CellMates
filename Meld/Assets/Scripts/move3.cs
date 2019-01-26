using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move3 : MonoBehaviour
{
    public float speed = 1;
    public float topSpeed = 2;
    private Rigidbody cube;
    Vector3 movement;

    public string inputHorizontal;
    public string inputVertical;

    // Start is called before the first frame update
    void Start()
    {
        cube = GetComponent<Rigidbody>();
        cube.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis (inputHorizontal) *20;
        float vertical = Input.GetAxis (inputVertical)* 20;

        Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);

        //cube.AddForce(movement * speed);

        
        // Controlling movement speed on the XZ plane
        movement = movement.normalized * speed;
        if (movement.magnitude > topSpeed)
            movement = movement.normalized * topSpeed;
        // This is to preserve Y movement so that gravity affects it properly
        movement.y = cube.velocity.y;
        cube.velocity = movement;


        //Vector3 vel = cube.velocity;
        //vel.x = horizontal;
        //vel.y = vertical;

//        cube.velocity = vel;
    }
}
