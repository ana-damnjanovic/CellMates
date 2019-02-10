using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class membraneController : MonoBehaviour
{
    public float maxSeparation = 1.5f;
    private GameObject player1;
    private GameObject player2;
    private playerBehaviour p1Behaviour;
    private playerBehaviour p2Behaviour;
    private GameObject membrane;
    private GameObject membraneSupportSphere;
    private Cloth cloth;

    public string membraneTag = "Membrane";
    public string membraneSupportSphereTag = "MembraneSupportSphere";
    public string player1Tag = "Player1";
    public string player2Tag = "Player2";

    void Start()
    {
        membrane = GameObject.FindWithTag(membraneTag);
        membraneSupportSphere = GameObject.FindWithTag(membraneSupportSphereTag);
        cloth = membrane.GetComponent<Cloth>();
        player1 = GameObject.FindWithTag(player1Tag);
        player2 = GameObject.FindWithTag(player2Tag);
        p1Behaviour = player1.GetComponent<playerBehaviour>();
        p2Behaviour = player2.GetComponent<playerBehaviour>();
    }

    void FixedUpdate()
    {
        bool p1Grounded = p1Behaviour.GetIsGrounded();
        bool p2Grounded = p2Behaviour.GetIsGrounded();

        Vector3 player1position = player1.transform.position;
        Vector3 player2position = player2.transform.position;
        player1position.y = 0;
        player2position.y = 0;
        float playerDistance = Vector3.Distance(player1position, player2position);

        Vector3 avg = (player1.transform.position + player2.transform.position) / 2;

        if (playerDistance > maxSeparation || (!p1Grounded) || (!p2Grounded))
        {
            // increase movement randomness while blob is in the air
            cloth.randomAcceleration = new Vector3(100f, 100f, 100f);
        }
        else
        {
            // default value for random movement
            cloth.randomAcceleration = new Vector3(10f, 10f, 10f);
        }
        // TODO: potentially add transformations to membraneSupportSphere scale
        membrane.transform.position = avg;
        cloth.ClearTransformMotion();
        membraneSupportSphere.transform.position = avg;
    }
}
