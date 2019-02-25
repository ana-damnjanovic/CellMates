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
        if (gameObject.tag == "Player1")
        {
            transform.position = cm.lastCheckpointPositionp1;
        }
        else {
            transform.position = cm.lastCheckpointPositionp2;
        }
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
                    GameObject.FindGameObjectWithTag("Player1").transform.position = cm.lastCheckpointPositionp1;
                    GameObject.FindGameObjectWithTag("Player2").transform.position = cm.lastCheckpointPositionp2;
                    GameObject.FindGameObjectWithTag("Membrane").transform.position = cm.lastCheckpointPositionmem;
                    GameObject.FindGameObjectWithTag("MembraneSupportSphere").transform.position = cm.lastCheckpointPositionmemsphere;
                    GameObject.FindGameObjectWithTag("Membrane").GetComponent<Cloth>().enabled = false;
                    GameObject.FindGameObjectWithTag("Membrane").GetComponent<Cloth>().enabled = true;
                }
            }
        }
    }
}
