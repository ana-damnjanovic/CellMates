using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndScreen : MonoBehaviour
{
    private playerBehaviour player1behaviour;
    private playerBehaviour player2behaviour;
    private bool checkpointset = false;
    private Light halo;
    private ParticleSystem particles;
    void Start()
    {
        player1behaviour = GameObject.FindGameObjectWithTag("Player1").GetComponent<playerBehaviour>();
        player2behaviour = GameObject.FindGameObjectWithTag("Player2").GetComponent<playerBehaviour>();
        halo = gameObject.transform.GetChild(0).GetComponent<Light>();
        particles = gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
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
                    halo.enabled = true;
                    particles.Play();
                }
            }
            if (player2Grounded)
            {
                player2GroundedHit = player2behaviour.GetGroundedHit();
                if (player2GroundedHit.transform.gameObject == gameObject && checkpointset == false)
                {
                    halo.enabled = true;
                    particles.Play();
                }
            }
        }
    }
}
