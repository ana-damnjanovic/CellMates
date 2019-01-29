using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move3 : MonoBehaviour
{
    public float speed = 1;
    public float topSpeed = 2;
    public float maxSeperation = 3;
    public float jump = 5;
    private Rigidbody cube;
    Vector3 movement;
    private GameObject player1;
    private GameObject player2;
    private GameObject membrane;

    public string inputHorizontal;
    public string inputVertical;
    public string player1Tag;
    public string player2Tag;
    public string membraneTag = "Membrane";

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
        if (Vector3.Distance(player1.transform.position, player2.transform.position) <= maxSeperation)
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


            //Vector3 vel = cube.velocity;
            //vel.x = horizontal;
            //vel.y = vertical;

            //        cube.velocity = vel;
        }
        else
        {
            Vector3 avg = (player1.transform.position + player2.transform.position) / 2;
            //player1.transform.position = avg;
            //player2.transform.position = avg;
            //membrane.transform.position = avg;
            avg.y += jump;

            //player1.GetComponent<Rigidbody>().AddForce(avg.x, avg.y, avg.z);
            //player2.GetComponent<Rigidbody>().AddForce(-avg.x, avg.y, -avg.z);

            player1.transform.position = avg;
            player2.transform.position = avg;
            //membrane.transform.position = avg;
        }
    }
}
