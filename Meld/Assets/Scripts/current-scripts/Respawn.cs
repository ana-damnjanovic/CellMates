using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private CheckpointManager cm;
    private playerBehaviour p1Behaviour;
    private playerBehaviour p2Behaviour;
    private GameObject p1;
    private GameObject p2;
    private GameObject membrane;
    private GameObject membraneSupportSphere;

    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
        p1Behaviour = GameObject.FindGameObjectWithTag("Player1").GetComponent<playerBehaviour>();
        p2Behaviour = GameObject.FindGameObjectWithTag("Player2").GetComponent<playerBehaviour>();
        p1 = GameObject.FindGameObjectWithTag("Player1");
        p2 = GameObject.FindGameObjectWithTag("Player2");
        membrane = GameObject.FindGameObjectWithTag("Membrane");
        membraneSupportSphere = GameObject.FindGameObjectWithTag("MembraneSupportSphere");
    }

    void respawn()
    {
        SoundEffectController.instance.Damage(); // play the "ouch" sound effect(s)
        p1.transform.position = new Vector3(cm.lastCheckpointPosition.x + 0.613803f, cm.lastCheckpointPosition.y, cm.lastCheckpointPosition.z + 0.20123f);
        p2.transform.position = new Vector3(cm.lastCheckpointPosition.x - 0.450197f, cm.lastCheckpointPosition.y, cm.lastCheckpointPosition.z - 0.46423f);
        membrane.transform.position = cm.lastCheckpointPosition;
        membraneSupportSphere.transform.position = cm.lastCheckpointPosition;
        membrane.GetComponent<Cloth>().enabled = false;
        membrane.GetComponent<Cloth>().enabled = true;
        p1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        p2.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        RaycastHit playerGroundedHit;
        if (p1Behaviour.GetIsGrounded())
        {
            playerGroundedHit = p1Behaviour.GetGroundedHit();
            if (playerGroundedHit.transform.CompareTag("DeathZone"))
            {
                respawn();
            }
        } if (p2Behaviour.GetIsGrounded())
        {
            playerGroundedHit = p2Behaviour.GetGroundedHit();
            if (playerGroundedHit.transform.CompareTag("DeathZone"))
            {
                respawn();
            }
        }
    }
}
