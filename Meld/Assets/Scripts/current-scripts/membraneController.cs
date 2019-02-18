using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class membraneController : MonoBehaviour
{
    public float maxSeparation = 2.85f;
    private GameObject player1;
    private GameObject player2;
    private playerBehaviour p1Behaviour;
    private playerBehaviour p2Behaviour;
    private GameObject membrane;
    private GameObject membraneSupportSphere;
    private SphereCollider membraneSupportCollider;
    private Cloth cloth;
    private Material material;

    public string membraneTag = "Membrane";
    public string membraneSupportSphereTag = "MembraneSupportSphere";
    public string player1Tag = "Player1";
    public string player2Tag = "Player2";

    private Color green = new Color(0.47f, 0.82f, 0.56f, 0.36f);
    private Color yellow = new Color(0.82f, 0.81f, 0.47f, 0.36f);
    private Color red = new Color(0.82f, 0.52f, 0.47f, 0.36f);

    void Start()
    {
        membrane = GameObject.FindWithTag(membraneTag);
        membraneSupportSphere = GameObject.FindWithTag(membraneSupportSphereTag);
        membraneSupportCollider = membraneSupportSphere.GetComponent<SphereCollider>();
        cloth = membrane.GetComponent<Cloth>();
        material = membrane.GetComponent<Renderer>().material;

        player1 = GameObject.FindWithTag(player1Tag);
        player2 = GameObject.FindWithTag(player2Tag);
        p1Behaviour = player1.GetComponent<playerBehaviour>();
        p2Behaviour = player2.GetComponent<playerBehaviour>();
    }

    void FixedUpdate()
    {
        bool p1Grounded = p1Behaviour.GetIsGrounded();
        bool p2Grounded = p2Behaviour.GetIsGrounded();
        bool p1Sticking = p1Behaviour.GetIsSticking();
        bool p2Sticking = p2Behaviour.GetIsSticking();

        float playerDistance = Vector3.Distance(player1.transform.position, player2.transform.position);
        float separation = playerDistance / maxSeparation;

        Vector3 avg = (player1.transform.position + player2.transform.position) / 2;
        bool sticking = p1Sticking || p2Sticking;

        if ((maxSeparation * 0.75f) <= playerDistance && playerDistance < maxSeparation)
        {
            material.color = Color.Lerp(green, yellow, separation);
        }

        else if (playerDistance > maxSeparation || (!p1Grounded && !sticking) || (!p2Grounded && !sticking))
        {
            // increase movement randomness while blob is in the air
            cloth.randomAcceleration = new Vector3(10f, 10f, 10f);
            material.color = green;
        }
        else
        {
            // default value for random movement
            cloth.externalAcceleration = new Vector3(0f, -10f, 0f);
        }
        membrane.transform.position = avg;
        cloth.ClearTransformMotion();
        membraneSupportSphere.transform.position = avg;
        membraneSupportCollider.radius = Math.Min(0.65f, 1/(4 * separation));
    }
}
