using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private CheckpointManager cm;
    private playerBehaviour playerbehaviour;
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
        transform.position = cm.lastCheckpointPosition;
        playerbehaviour = gameObject.GetComponent<playerBehaviour>();
    }

    void FixedUpdate()
    {
        RaycastHit playerGroundedHit;
        bool playerGrounded = playerbehaviour.GetIsGrounded();
        if (playerGrounded)
        {
            playerGroundedHit = playerbehaviour.GetGroundedHit();
            if (playerGroundedHit.collider)
            {
                if (playerGroundedHit.transform.CompareTag("DeathZone"))
                {
                    GameObject.FindGameObjectWithTag("Player1").transform.position = cm.lastCheckpointPosition;
                    GameObject.FindGameObjectWithTag("Player2").transform.position = cm.lastCheckpointPosition;
                    GameObject.FindGameObjectWithTag("Membrane").transform.position = cm.lastCheckpointPosition;
                    GameObject.FindGameObjectWithTag("MembraneSupportSphere").transform.position = cm.lastCheckpointPosition;
                }
            }
        }
    }
}
