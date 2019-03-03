using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCleanUnityCloth2 : MonoBehaviour
{
    public float topSpeed = GameManager.topSpeed;
    public float maxSeparation = GameManager.maxSeparation;
    public float jump = 5;
    Rigidbody p1RigidBody;
    Rigidbody p2RigidBody;
    private GameObject player1;
    private GameObject player2;
    private playerBehaviour p1Behaviour;
    private playerBehaviour p2Behaviour;
    private LayerMask layerMask;
    private float playerDistance = 0;
    private bool p1AbleToJump = false;
    private bool p2AbleToJump = false;

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

    public bool p1Maze = false;
    public bool p2Maze = false;
    public bool p1EndMaze = false;
    public bool p2EndMaze = false;

    public float jumpMagnitude = 500f;

    public TensionSlider TensionSlider1;
    public TensionSlider TensionSlider2;

    public AudioClip jumpSound;
    private AudioSource source;
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

        source = GetComponent<AudioSource>();
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

        p1AbleToJump = Input.GetButtonDown(p1JumpButton);
        p2AbleToJump = Input.GetButtonDown(p2JumpButton);

        //TensionSlider1.SetTension(playerDistance);
        //TensionSlider2.SetTension(playerDistance);
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
            bool hit = Physics.SphereCast(player.transform.position, player.GetComponent<SphereCollider>().radius, dir, out rayHit, 1, layerMask);
            if (hit && rayHit.transform.CompareTag("stickable"))
            {
                canStick = true;
            }
            // TODO: uncomment and improve after alpha
            //else if (hit && rayHit.transform.CompareTag("stickable-move"))
            //{
            //    canStick = true;
            //    float p1Horizontal = Input.GetAxis(p1HorizontalInput);
            //    float p1Vertical = Input.GetAxis(p1VerticalInput);

            //    float p2Horizontal = Input.GetAxis(p2HorizontalInput);
            //    float p2Vertical = Input.GetAxis(p2VerticalInput);

            //    Vector3 move;
            //    if (player.transform.name == "Player 1")
            //    {
            //        move = new Vector3(p1Horizontal, 0.0f, p1Vertical);
            //    }
            //    else
            //    {
            //        move = new Vector3(p2Horizontal, 0.0f, p2Vertical);
            //    }

            //    move.y = rayHit.transform.GetComponent<Rigidbody>().velocity.y;
            //    rayHit.transform.GetComponent<Rigidbody>().velocity = move;
            //}
        }

        return canStick;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        p1Behaviour.SetVelocity(Input.GetAxis(p1HorizontalInput), Input.GetAxis(p1VerticalInput));
        p2Behaviour.SetVelocity(Input.GetAxis(p2HorizontalInput), Input.GetAxis(p2VerticalInput));

        // bool p1Maze = false;
        // bool p2Maze = false;
        // bool p1EndMaze = false;
        // bool p2EndMaze = false;

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
            if (p1GroundedHit.transform.CompareTag("maze"))
            {
                p1Maze = true;
            }
            else
            {
                p1Maze = false;
            }
            if (p1GroundedHit.transform.CompareTag("EndMaze"))
            {
                p1EndMaze = true;
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
            if (p2GroundedHit.transform.CompareTag("maze"))
            {
                p2Maze = true;
            }
            else
            {
                p2Maze = false;
            }
            if (p2GroundedHit.transform.CompareTag("EndMaze"))
            {
                p2EndMaze = true;
            }
        }

        Camera mazecam = GameObject.FindWithTag("mazecam").GetComponent<Camera>();
        Camera maincam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //Camera revcam = GameObject.FindWithTag("ReverseCamera").GetComponent<Camera>();

        // if (p1Maze || p2Maze)
        // {
        //     maincam.enabled = false;
        //     mazecam.enabled = true;
        //     mazecam.GetComponent<CameraController>().enabled = true;
        // }
        // else
        // {
        //     maincam.enabled = true;
        //     mazecam.enabled = false;
        //     mazecam.GetComponent<CameraController>().enabled = false;
        // }



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
        //player1position.y = 0;
        //player2position.y = 0;


        bool p1CanStick = createStickingRay(player1);
        bool p2CanStick = createStickingRay(player2);

        if (Input.GetButton(p1StickButton) && p1CanStick)
        {
            player1.GetComponent<Rigidbody>().drag = 0;
            player2.GetComponent<Rigidbody>().drag = 0;
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            p1Behaviour.SetIsSticking(true);
        } else if (p1CanStick && (!p1Behaviour.GetIsGrounded() || !p2Behaviour.GetIsGrounded())) {
            //player1.GetComponent<Rigidbody>().drag = 10;
            //player2.GetComponent<Rigidbody>().drag = 10;
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p1Behaviour.SetIsSticking(false);
        }
        else
        {
            player1.GetComponent<Rigidbody>().drag = 0;
            player2.GetComponent<Rigidbody>().drag = 0;
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p1Behaviour.SetIsSticking(false);
        }
        if (Input.GetButton(p2StickButton) && p2CanStick)
        {
            player1.GetComponent<Rigidbody>().drag = 0;
            player2.GetComponent<Rigidbody>().drag = 0;
            player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            p2Behaviour.SetIsSticking(true);
        } else if (p2CanStick && (!p1Behaviour.GetIsGrounded() || !p2Behaviour.GetIsGrounded())) 
        {
            //player1.GetComponent<Rigidbody>().drag = 10;
            //player2.GetComponent<Rigidbody>().drag = 10;
            player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p2Behaviour.SetIsSticking(false);
        }
        else
        {
            player1.GetComponent<Rigidbody>().drag = 0;
            player2.GetComponent<Rigidbody>().drag = 0;
            player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p2Behaviour.SetIsSticking(false);
        }


        Vector3 player1positionNoY = player1position;
        Vector3 player2positionNoY = player2position;
        player1positionNoY.y = 0;
        player2positionNoY.y = 0;

        playerDistance = Vector3.Distance(player1positionNoY, player2positionNoY);
        Vector3 avg = (player1.transform.position + player2.transform.position) / 2;

        if (p1Grounded || p2Grounded) {
            player1.GetComponent<SpringJoint>().maxDistance = GameManager.maxSpringDistance;
        }


        bool sticking = p1Behaviour.GetIsSticking() || p2Behaviour.GetIsSticking();

        // Players are max seperated, or in the air
        if (playerDistance > (maxSeparation - 0.5) || !p1Grounded || !p2Grounded)
        {   
            // Players are trying to jump
            if (p1AbleToJump || p2AbleToJump){

                // Players aren't sticking, and at least one of them is on the ground
                if (!sticking && (p1Grounded || p2Grounded)) {

                    Vector3 pullCenter = avg;

                    // If both players are on the ground, pull them upwards
                    if (p1Grounded && p2Grounded)
                    {
                        pullCenter.y = jump;
                    }

                    // Remove vertical portions so that we don't lose upward force
                    Vector3 temp = player1.transform.position;
                    //temp.y = 0;
                    Vector3 temp1 = player2.transform.position;
                    //temp1.y = 0;

                    //player1.GetComponent<Rigidbody>().AddForce((pullCenter - temp).normalized * jumpMagnitude);
                    //player2.GetComponent<Rigidbody>().AddForce((pullCenter - temp1).normalized * jumpMagnitude);
                    player1.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpMagnitude);
                    player2.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpMagnitude);
                    source.PlayOneShot(jumpSound, 0.25f);
                    player1.GetComponent<SpringJoint>().maxDistance = 0;


                } else if (p1Behaviour.GetIsSticking() && (Input.GetButton(p1StickButton) || Input.GetButton(p2StickButton)) ){//&& playerDistance > maxSeparation) {
                    Vector3 pull = avg;
                    pull = pull  - player2.transform.position;
                    pull.x = pull.x /2;
                    pull.z = pull.z /2;
                    player2.GetComponent<Rigidbody>().AddForce((pull).normalized * jumpMagnitude);
                } else if (p2Behaviour.GetIsSticking() && (Input.GetButton(p1StickButton) || Input.GetButton(p2StickButton)) ){//&& playerDistance > maxSeparation) {
                    Vector3 pull = avg;
                    pull = pull  - player1.transform.position;
                    pull.x = pull.x /2;
                    pull.z = pull.z /2;
                    
                    player1.GetComponent<Rigidbody>().AddForce((pull).normalized * jumpMagnitude);
                } else if (p1Behaviour.GetIsSticking() && playerDistance > maxSeparation) {
                    player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * 20 * topSpeed);
                } else if (p2Behaviour.GetIsSticking() && playerDistance > maxSeparation) {
                    player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * 20 * topSpeed);
                }

                p1AbleToJump = false;
                p2AbleToJump = false;
            } else if (playerDistance > maxSeparation) {
                //player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * 20 * topSpeed);
                //player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * 20 * topSpeed);
            }

        }
    }
}
