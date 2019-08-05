using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager cm;
    private playerBehaviour player1behaviour;
    private playerBehaviour player2behaviour;
    private bool checkpointset = false;
    private Light halo;
    private ParticleSystem particles;
    private Text text;
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
        player1behaviour = GameObject.FindGameObjectWithTag("Player1").GetComponent<playerBehaviour>();
        player2behaviour = GameObject.FindGameObjectWithTag("Player2").GetComponent<playerBehaviour>();
        halo = gameObject.transform.GetChild(0).GetComponent<Light>();
        particles = gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
        text = gameObject.transform.Find("Canvas").gameObject.transform.Find("CheckpointText").GetComponent<Text>();
        text.enabled = false;
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
                if (player1GroundedHit.transform.gameObject == gameObject && checkpointset == false)
                {
                    cm.lastCheckpointPosition = this.transform.position;
                    cm.lastCheckpointPosition.y = GameObject.FindGameObjectWithTag("Player1").transform.position.y + 0.30065f;
                    halo.enabled = true;
                    checkpointset = true;
                    particles.Play();
                    text.enabled = true;
                }
            }
            if (player2Grounded)
            {
                player2GroundedHit = player2behaviour.GetGroundedHit();
                if (player2GroundedHit.transform.gameObject == gameObject && checkpointset == false)
                {
                    cm.lastCheckpointPosition = this.transform.position;
                    cm.lastCheckpointPosition.y = GameObject.FindGameObjectWithTag("Player2").transform.position.y + 0.30065f;
                    checkpointset = true;
                    halo.enabled = true;
                    particles.Play();
                    text.enabled = true;
                }
            }
        }
    }
}
