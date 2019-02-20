using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCleanUnityCloth2 : MonoBehaviour
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
    private LayerMask layerMask;
    private float playerDistance = 0;

    public string player1Tag = "Player1";
    public string p1HorizontalInput = "HorizontalP1";
    public string p1VerticalInput = "VerticalP1";
    public string p1StickButton = "StickP1";
    public string p1JumpButton = "JumpP1";

    public string player2Tag = "Player2";
    public string p2HorizontalInput = "HorizontalP2";
    public string p2VerticalInput = "VerticalP2";
    public string p2StickButton = "StickP2";
    public string p2JumpButton = "JumpP2";

    public float jumpMagnitude = 500f;

    public TensionSlider TensionSlider1;
    public TensionSlider TensionSlider2;
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

        // Makes sticking rays ignore players, cell membrane, and support structure
        LayerMask playerLayer = 1 << 9;
        LayerMask cellLayer = 1 << 10;
        LayerMask supportLayer = 1 << 11;
        layerMask = ~(playerLayer | cellLayer | supportLayer);
    }

    private void Update()
    {

        Vector3 down = transform.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(player1.transform.position, down, Color.green);
        Debug.DrawRay(player2.transform.position, down, Color.green);

        Vector3[] directions = {Vector3.up, Vector3.forward, Vector3.left, Vector3.right, Vector3.back};

        foreach (Vector3 dir in directions) {
            Vector3 fwd = transform.TransformDirection(dir) * 50;
            Debug.DrawRay(player1.transform.position, fwd, Color.red);
            Debug.DrawRay(player2.transform.position, fwd, Color.red);
        }
        TensionSlider1.SetTension(playerDistance);
        TensionSlider2.SetTension(playerDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 2, false);
    }

    private bool createStickingRay(GameObject player) {
        
        RaycastHit rayHit;
        bool canStick = false;
        canStick = false;

        Vector3[] directions = {Vector3.up, Vector3.forward, Vector3.left, Vector3.right, Vector3.back};

        foreach (Vector3 dir in directions) {
            bool hit = Physics.Raycast(player.transform.position, dir, out rayHit, 1, layerMask);
            if (hit && rayHit.collider.CompareTag("stickable"))
            {
                canStick = true;
            }
        }

        return canStick;
    }

    private void calculatePlayerVelocity(float horizontal, float vertical, Rigidbody rb)
    {
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);

        // Controlling movement speed on the XZ plane
        movement = movement.normalized * speed;
        if (movement.magnitude > topSpeed)
            movement = movement.normalized * topSpeed;
        // This is to preserve Y movement so that gravity affects it properly
        movement.y = rb.velocity.y;

        rb.velocity = movement;
    }

    private bool setPlayerOnOtherPlayer(GameObject player,  playerBehaviour pb, string otherPlayerTag, out bool playerGrounded)
    {
        RaycastHit playerGroundedHit;
        playerGrounded = pb.GetIsGrounded();
        bool playerOnOtherPlayer = false;
        if (playerGrounded)
        {
            playerGroundedHit = pb.GetGroundedHit();
            if (playerGroundedHit.rigidbody)
            {
                if (playerGroundedHit.rigidbody.CompareTag(otherPlayerTag))
                {
                    playerGrounded = false;
                    playerOnOtherPlayer = true;
                }
                else
                {
                    playerOnOtherPlayer = false;
                }
            }
        }

        return playerOnOtherPlayer;
    }

    private void preventPlayerSinking(Rigidbody playerRb, bool playerGrounded)
    {
        // This should prevent the player from sinking to the ground
        playerRb.useGravity = !playerGrounded;
        // This should also prevent the player from sinking
        if (playerGrounded)
        {
            var temp1 = playerRb.velocity;
            temp1.y = Math.Max(0, temp1.y);
            playerRb.velocity = temp1;
        }
    }

    private void calculatePlayerSticking(string playerStickButton, bool playerCanStick, GameObject player, GameObject otherPlayer, playerBehaviour playerBehaviour, playerBehaviour otherPlayerBehaviour)
    {
        if (Input.GetButton(playerStickButton) && playerCanStick)
        {
            player.GetComponent<Rigidbody>().drag = 0;
            otherPlayer.GetComponent<Rigidbody>().drag = 0;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            playerBehaviour.SetIsSticking(true);
        }
        else if (playerCanStick && (!playerBehaviour.GetIsGrounded() || !otherPlayerBehaviour.GetIsGrounded()))
        {
            player.GetComponent<Rigidbody>().drag = 10;
            otherPlayer.GetComponent<Rigidbody>().drag = 10;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            playerBehaviour.SetIsSticking(false);
        }
        else
        {
            player.GetComponent<Rigidbody>().drag = 0;
            otherPlayer.GetComponent<Rigidbody>().drag = 0;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            playerBehaviour.SetIsSticking(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (true || (Vector3.Distance(player1.transform.position, player2.transform.position) <= maxSeparation))
        {
            float p1Horizontal = Input.GetAxis(p1HorizontalInput) * 20;
            float p1Vertical = Input.GetAxis(p1VerticalInput) * 20;

            calculatePlayerVelocity(p1Horizontal, p1Vertical, p1RigidBody);

            float p2Horizontal = Input.GetAxis(p2HorizontalInput) * 20;
            float p2Vertical = Input.GetAxis(p2VerticalInput) * 20;

            calculatePlayerVelocity(p2Horizontal, p2Vertical, p2RigidBody);
        }

        bool p1Grounded;
        bool p1onp2 = setPlayerOnOtherPlayer(player1, p1Behaviour, player2Tag, out p1Grounded);
        bool p2Grounded;
        bool p2onp1 = setPlayerOnOtherPlayer(player2, p2Behaviour, player1Tag, out p2Grounded);

        preventPlayerSinking(p1RigidBody, p1Grounded);
        preventPlayerSinking(p2RigidBody, p2Grounded);

        bool p1CanStick = createStickingRay(player1);
        bool p2CanStick = createStickingRay(player2);
 
        Vector3 player1position = player1.transform.position;
        Vector3 player2position = player2.transform.position;

        calculatePlayerSticking(p1StickButton, p1CanStick, player1, player2, p1Behaviour, p2Behaviour);
        calculatePlayerSticking(p2StickButton, p2CanStick, player2, player1, p2Behaviour, p1Behaviour);

        playerDistance = Vector3.Distance(player1position, player2position);
        Vector3 avg = (player1.transform.position + player2.transform.position) / 2;

        bool sticking = p1Behaviour.GetIsSticking() || p2Behaviour.GetIsSticking();
        if (playerDistance > (maxSeparation - 0.5) || !p1Grounded || !p2Grounded)
        {   
            if (Input.GetButton(p1JumpButton) || Input.GetButton(p2JumpButton)){
                if (!sticking) {
    
                    Vector3 pullCenter = avg;
                    if (p1Grounded && p2Grounded)
                    {
                        pullCenter.y = jump;
                    }
                    player1.GetComponent<Rigidbody>().AddForce((pullCenter - player1.transform.position).normalized * jumpMagnitude);
                    player2.GetComponent<Rigidbody>().AddForce((pullCenter - player2.transform.position).normalized * jumpMagnitude);
                } else if (p1Behaviour.GetIsSticking() && (Input.GetButton(p1StickButton) || Input.GetButton(p2StickButton)) ){//&& playerDistance > maxSeparation) {
                    Vector3 pull = avg;
                    pull = pull  - player2.transform.position;
                    pull.x = pull.x /2;
                    pull.z = pull.z /2;
                    player2.GetComponent<Rigidbody>().AddForce((pull).normalized * jumpMagnitude/2);
                } else if (p2Behaviour.GetIsSticking() && (Input.GetButton(p1StickButton) || Input.GetButton(p2StickButton)) ){//&& playerDistance > maxSeparation) {
                    Vector3 pull = avg;
                    pull = pull  - player1.transform.position;
                    pull.x = pull.x /2;
                    pull.z = pull.z /2;
                    player1.GetComponent<Rigidbody>().AddForce((pull).normalized * jumpMagnitude/2);
                } else if (p1Behaviour.GetIsSticking() && playerDistance > maxSeparation) {
                    player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * 20 * topSpeed);
                } else if (p2Behaviour.GetIsSticking() && playerDistance > maxSeparation) {
                    player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * 20 * topSpeed);
                }
            } else if (playerDistance > maxSeparation) {
                //player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * 20 * topSpeed);
                //player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * 20 * topSpeed);
            }

        }
    }
}
