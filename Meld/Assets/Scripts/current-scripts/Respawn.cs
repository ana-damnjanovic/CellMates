using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private CheckpointManager cm;
    private playerBehaviour p1Behaviour;
    private playerBehaviour p2Behaviour;
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
        p1Behaviour = GameObject.FindGameObjectWithTag("Player1").GetComponent<playerBehaviour>();
        p2Behaviour = GameObject.FindGameObjectWithTag("Player2").GetComponent<playerBehaviour>();
    }

    void FixedUpdate()
    {
        RaycastHit playerGroundedHit;
        if (p1Behaviour.GetIsGrounded())
        {
            playerGroundedHit = p1Behaviour.GetGroundedHit();
            if (playerGroundedHit.transform.CompareTag("DeathZone"))
            {
                GameObject.FindGameObjectWithTag("Player1").transform.position = new Vector3(cm.lastCheckpointPosition.x + 0.613803f, cm.lastCheckpointPosition.y, cm.lastCheckpointPosition.z + 0.20123f);
                GameObject.FindGameObjectWithTag("Player2").transform.position = new Vector3(cm.lastCheckpointPosition.x - 0.450197f, cm.lastCheckpointPosition.y, cm.lastCheckpointPosition.z - 0.46423f);
                GameObject.FindGameObjectWithTag("Membrane").transform.position = cm.lastCheckpointPosition;
                GameObject.FindGameObjectWithTag("MembraneSupportSphere").transform.position = cm.lastCheckpointPosition;
            }
        } else if (p2Behaviour.GetIsGrounded())
        {
            playerGroundedHit = p2Behaviour.GetGroundedHit();
            if (playerGroundedHit.transform.CompareTag("DeathZone"))
            {
                GameObject.FindGameObjectWithTag("Player1").transform.position = new Vector3(cm.lastCheckpointPosition.x + 0.613803f, cm.lastCheckpointPosition.y, cm.lastCheckpointPosition.z + 0.20123f);
                GameObject.FindGameObjectWithTag("Player2").transform.position = new Vector3(cm.lastCheckpointPosition.x - 0.450197f, cm.lastCheckpointPosition.y, cm.lastCheckpointPosition.z - 0.46423f);
                GameObject.FindGameObjectWithTag("Membrane").transform.position = cm.lastCheckpointPosition;
                GameObject.FindGameObjectWithTag("MembraneSupportSphere").transform.position = cm.lastCheckpointPosition;
            }
        }
    }
}
