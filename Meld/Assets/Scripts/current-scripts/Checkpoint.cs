using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager cm;
    private playerBehaviour player1behaviour;
    private playerBehaviour player2behaviour;
    private bool checkpointset = false;
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
        player1behaviour = GameObject.FindGameObjectWithTag("Player1").GetComponent<playerBehaviour>();
        player2behaviour = GameObject.FindGameObjectWithTag("Player2").GetComponent<playerBehaviour>();
    }

    void FixedUpdate()
    {
        if (checkpointset == false)
        {
            RaycastHit player1GroundedHit;
            RaycastHit player2GroundedHit;
            bool player1Grounded = player1behaviour.GetIsGrounded();
            bool player2Grounded = player2behaviour.GetIsGrounded();
            if (player1Grounded)
            {
                player1GroundedHit = player1behaviour.GetGroundedHit();
                if (player1GroundedHit.collider.gameObject == gameObject)
                {
                    cm.lastCheckpointPosition = GameObject.FindGameObjectWithTag("Player1").transform.position;
                    checkpointset = true;
                }
            }
            if (player2Grounded)
            {
                player2GroundedHit = player2behaviour.GetGroundedHit();
                if (player2GroundedHit.collider.gameObject == gameObject)
                {
                    cm.lastCheckpointPosition = GameObject.FindGameObjectWithTag("Player2").transform.position;
                    checkpointset = true;
                }
            }
        }
    }
}
