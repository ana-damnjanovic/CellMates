using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingFunctions : MonoBehaviour
{
    Rigidbody p1RigidBody;
    Rigidbody p2RigidBody;
    private GameObject player1;
    private GameObject player2;
    private playerBehaviour p1Behaviour;
    private playerBehaviour p2Behaviour;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindWithTag(GameManager.player1Tag);
        p1RigidBody = player1.GetComponent<Rigidbody>();
        p1Behaviour = player1.GetComponent<playerBehaviour>(); 

        player2 = GameObject.FindWithTag(GameManager.player2Tag);
        p2RigidBody = player2.GetComponent<Rigidbody>();
        p2Behaviour = player2.GetComponent<playerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 down = transform.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(player1.transform.position, down, Color.green);
        Debug.DrawRay(player2.transform.position, down, Color.green);

        Vector3[] directions = {Vector3.up, Vector3.forward, Vector3.left, Vector3.right, Vector3.back};

        foreach (Vector3 dir in directions) {
            Vector3 fwd = transform.TransformDirection(dir) * 50;
            Debug.DrawRay(player1.transform.position, fwd, Color.red);
            Debug.DrawRay(player2.transform.position, fwd, Color.red);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 2, false);
    }
}
