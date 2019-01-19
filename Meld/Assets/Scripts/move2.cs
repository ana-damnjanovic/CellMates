using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move2 : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody cube;

    // Start is called before the first frame update
    void Start()
    {
        cube = GetComponent<Rigidbody>();
        cube.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis ("Horizontal2") *20;
        float vertical = Input.GetAxis ("Vertical2")* 20;

        Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);

        cube.AddForce(movement * speed);

        //Vector3 vel = cube.velocity;
        //vel.x = horizontal;
        //vel.y = vertical;

//        cube.velocity = vel;
    }
}
