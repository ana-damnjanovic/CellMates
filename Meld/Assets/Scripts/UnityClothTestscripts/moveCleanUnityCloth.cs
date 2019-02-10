using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCleanUnityCloth : MonoBehaviour
{
    public float speed = 1;
    public float topSpeed = 3;
    public float maxSeparation = 1.5f;
    public float jump = 5;
    Rigidbody p1RigidBody;
    Rigidbody p2RigidBody;
    Vector3 movement;
    private GameObject player1;
    private GameObject player2;
    private playerBehaviour p1Behaviour;
    private playerBehaviour p2Behaviour;

    public string player1Tag = "Player1";
    public string p1HorizontalInput = "Horizontal";
    public string p1VerticalInput = "Vertical";
    public string p1StickButton = "Fire1";

    public string player2Tag = "Player2";
    public string p2HorizontalInput = "Horizontal2";
    public string p2VerticalInput = "Vertical2";
    public string p2StickButton = "Fire2";

    public float jumpMagnitude = 500f;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindWithTag(player1Tag);
        p1RigidBody = player1.GetComponent<Rigidbody>();
        p1RigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        p1Behaviour = player1.GetComponent<playerBehaviour>(); 


        player2 = GameObject.FindWithTag(player2Tag);
        p2RigidBody = player2.GetComponent<Rigidbody>();
        p2RigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        p2Behaviour = player2.GetComponent<playerBehaviour>();
    }

    private void Update()
    {

        Vector3 down = transform.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(player1.transform.position, down, Color.green);
        Debug.DrawRay(player2.transform.position, down, Color.green);
        Vector3 fwd = transform.TransformDirection(Vector3.forward) * 50;
        Debug.DrawRay(player1.transform.position, fwd, Color.red);
        Debug.DrawRay(player2.transform.position, fwd, Color.red);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 2, false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Makes sticking rays ignore players, cell membrane, and support structure
        LayerMask playerLayer = 1 << 9;
        LayerMask cellLayer = 1 << 10;
        LayerMask supportLayer = 1 << 11;
        LayerMask layerMask = ~(playerLayer | cellLayer | supportLayer);
        // Debug.Log(~(playerLayer | cellLayer | supportLayer));

        RaycastHit p1FwdHit;
        bool p1CanStick = false;
        Vector3 p1Fwd = player1.transform.TransformDirection(Vector3.forward);
        bool p1HitFwd = Physics.Raycast(player1.transform.position, Vector3.forward, out p1FwdHit, 3, layerMask);

        if (p1HitFwd && p1FwdHit.collider.CompareTag("stickable"))
        {
            p1CanStick = true;
        }
        else
        {
            p1CanStick = false;
        }

        RaycastHit p2FwdHit;
        bool p2CanStick = false;
        Vector3 p2Fwd = player2.transform.TransformDirection(Vector3.forward);
        bool p2HitFwd = Physics.Raycast(player2.transform.position, Vector3.forward, out p2FwdHit, 3, layerMask);

        if (p2HitFwd && p2FwdHit.collider.CompareTag("stickable"))
        {
            p2CanStick = true;
        }
        else
        {
            p2CanStick = false;
        }


        if (Input.GetButton(p1StickButton) && p1CanStick)
        {
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            p1Behaviour.SetIsSticking(true);
        }
        else
        {
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p1Behaviour.SetIsSticking(false);
        }
        if (Input.GetButton(p2StickButton) && p2CanStick)
        {
            player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            p2Behaviour.SetIsSticking(true);
        }
        else
        {
            player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p2Behaviour.SetIsSticking(false);
        }



        if (true || (Vector3.Distance(player1.transform.position, player2.transform.position) <= maxSeparation))
        {
            float p1Horizontal = Input.GetAxis(p1HorizontalInput) * 20;
            float p1Vertical = Input.GetAxis(p1VerticalInput) * 20;

            Vector3 p1Movement = new Vector3(p1Horizontal, 0.0f, p1Vertical);

            // Controlling movement speed on the XZ plane
            p1Movement = p1Movement.normalized * speed;
            if (p1Movement.magnitude > topSpeed)
                p1Movement = p1Movement.normalized * topSpeed;
            // This is to preserve Y movement so that gravity affects it properly
            p1Movement.y = p1RigidBody.velocity.y;

            p1RigidBody.velocity = p1Movement;

            float p2Horizontal = Input.GetAxis(p2HorizontalInput) * 20;
            float p2Vertical = Input.GetAxis(p2VerticalInput) * 20;

            Vector3 p2Movement = new Vector3(p2Horizontal, 0.0f, p2Vertical);

            // Controlling movement speed on the XZ plane
            p2Movement = p2Movement.normalized * speed;
            if (p2Movement.magnitude > topSpeed)
                p2Movement = p2Movement.normalized * topSpeed;
            // This is to preserve Y movement so that gravity affects it properly
            p2Movement.y = p2RigidBody.velocity.y;

            p2RigidBody.velocity = p2Movement;
        }


        RaycastHit p1GroundedHit;
        var p1Ray = new Ray(player1.transform.position, Vector3.down);
        bool p1Grounded = p1Behaviour.GetIsGrounded();
        bool p1onp2 = false;
        if (p1Grounded)
        {
            p1GroundedHit = p1Behaviour.GetGroundedHit();
            if (p1GroundedHit.rigidbody)
            {
                if (p1GroundedHit.rigidbody.CompareTag(player2Tag))
                {
                    p1Grounded = false;
                    p1onp2 = true;
                }
                else
                {
                    p1onp2 = false;
                }
            }
        }


        // This should prevent the player from sinking to the ground
        p1RigidBody.useGravity = !p1Grounded;
        // This should extra prevent the player from sinking
        if (p1Grounded)
        {
            var temp1 = p1RigidBody.velocity;
            temp1.y = Math.Max(0, temp1.y);
            p1RigidBody.velocity = temp1;
        }

        RaycastHit p2GroundedHit;
        bool p2Grounded = p2Behaviour.GetIsGrounded();
        bool p2onp1 = false;
        if (p2Grounded)
        {
            p2GroundedHit = p2Behaviour.GetGroundedHit();
            if (p2GroundedHit.rigidbody)
            {
                if (p2GroundedHit.rigidbody.CompareTag(player1Tag))
                {
                    p2Grounded = false;
                    p2onp1 = true;
                }
                else
                {
                    p2onp1 = false;
                }
            }
        }


        // This should prevent the player from sinking to the ground
        p2RigidBody.useGravity = !p2Grounded;
        // This should extra prevent the player from sinking
        if (p2Grounded)
        {
            var temp2 = p2RigidBody.velocity;
            temp2.y = Math.Max(0, temp2.y);
            p2RigidBody.velocity = temp2;
        }
        Vector3 player1position = player1.transform.position;
        Vector3 player2position = player2.transform.position;
        player1position.y = 0;
        player2position.y = 0;

        float playerDistance = Vector3.Distance(player1position, player2position);
        Vector3 avg = (player1.transform.position + player2.transform.position) / 2;

        // start pulling players together gently when they're grounded and close to max separation
        //if ((maxSeparation * 0.25f) <= playerDistance && playerDistance < maxSeparation)
        //{
        //    print("case 1");
        //    // TODO: add more checks for special cases (sticking or stacked players)
        //    float weakJumpMagnitude = 10;
        //    if ((maxSeparation * 0.65f) <= playerDistance && playerDistance < maxSeparation)
        //    {
        //        weakJumpMagnitude = 15;
        //    }

        //    player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * weakJumpMagnitude);
        //    player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * weakJumpMagnitude);
        //}

        if (playerDistance > maxSeparation || (!p1Grounded) || (!p2Grounded))
        {
            Vector3 pullCenter = avg;
            if (p1Grounded && p2Grounded)
            {
                pullCenter.y = jump;
            }
            player1.GetComponent<Rigidbody>().AddForce((pullCenter - player1.transform.position).normalized * jumpMagnitude);
            player2.GetComponent<Rigidbody>().AddForce((pullCenter - player2.transform.position).normalized * jumpMagnitude);
        }
    }
}
