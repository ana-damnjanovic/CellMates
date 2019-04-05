using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private bool squint = false;
    private bool isDanking = false;

    private float jumpMagnitude = GameManager.jumpMagnitude;
    private float stickingJumpMagnitude = GameManager.stickingJumpMagnitude;

    public TensionSlider TensionSlider1;
    public TensionSlider TensionSlider2;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindWithTag(player1Tag);
        p1RigidBody = player1.GetComponent<Rigidbody>();
        p1Behaviour = player1.GetComponent<playerBehaviour>(); 


        player2 = GameObject.FindWithTag(player2Tag);
        p2RigidBody = player2.GetComponent<Rigidbody>();
        p2Behaviour = player2.GetComponent<playerBehaviour>();

        // Makes sticking rays ignore players, cell membrane, and support structure
        LayerMask playerLayer = 1 << 9;
        LayerMask cellLayer = 1 << 10;
        LayerMask supportLayer = 1 << 11;
        layerMask = ~(playerLayer | cellLayer | supportLayer);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Restart")) {
            SceneManager.LoadScene("Lab", LoadSceneMode.Single);
        }
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

        if ((Input.GetButtonDown(p1StickButton) && p1Behaviour.GetIsSticking()) || 
            (Input.GetButtonDown(p2StickButton) && p2Behaviour.GetIsSticking()))
        {
            SoundEffectController.instance.Stick();
        }


        //TensionSlider1.SetTension(playerDistance);
        //TensionSlider2.SetTension(playerDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 2, false);
    }

    private bool[] createStickingRay(GameObject player) {
        
        RaycastHit rayHit;
        bool canStick = false;
        bool canPull = false;

        Vector3[] directions = {Vector3.up, Vector3.forward, Vector3.left, Vector3.right, Vector3.back, Vector3.down};

        foreach (Vector3 dir in directions) {
            bool hit = Physics.SphereCast(player.transform.position, player.GetComponent<SphereCollider>().radius, dir, out rayHit, 1, layerMask);
            if (hit && rayHit.transform.CompareTag("stickable"))
            {
                canStick = true;
            }
            if (hit && rayHit.transform.CompareTag("pullable"))
            {
                canPull = true;
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
        if (canPull) {
            canStick = false;
        }
        bool [] ret = new bool[2];
        ret[0] = canStick;
        ret[1] = canPull;
        return ret;
    }

    void squinting() {
        GameObject[] eyelids = GameObject.FindGameObjectsWithTag("eyelids");
        foreach (GameObject eyelid in eyelids) {
            eyelid.GetComponent<MeshRenderer>().enabled = squint && !isDanking;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        p1Behaviour.SetVelocity(Input.GetAxis(p1HorizontalInput), Input.GetAxis(p1VerticalInput), p2Behaviour.GetIsSticking());
        p2Behaviour.SetVelocity(Input.GetAxis(p2HorizontalInput), Input.GetAxis(p2VerticalInput), p1Behaviour.GetIsSticking());

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

        bool[] p1Tuple = createStickingRay(player1);
        bool[] p2Tuple = createStickingRay(player2);

        bool p1CanStick = p1Tuple[0];
        bool p1CanPull = p1Tuple[1];
        bool p2CanStick = p2Tuple[0];
        bool p2CanPull = p2Tuple[1];
        p1Behaviour.SetIsPulling(false);
        p2Behaviour.SetIsPulling(false);
        if (!Input.GetButton(p1StickButton)){
            if (player1.GetComponent<FixedJoint>() != null) {
                Destroy(player1.GetComponent<FixedJoint>());
            }
        }

        if (!Input.GetButton(p2StickButton)){
            if (player2.GetComponent<FixedJoint>() != null) {
                Destroy(player2.GetComponent<FixedJoint>());
            }
        }

        if (Input.GetButton(p1StickButton) && p1CanStick)
        {
            player1.GetComponent<Rigidbody>().drag = 0;
            player2.GetComponent<Rigidbody>().drag = 0;
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            p1Behaviour.SetIsSticking(true);

        } else if (Input.GetButton(p1StickButton) && p1CanPull) {
            p1Behaviour.SetIsPulling(true);
            player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p1Behaviour.SetIsSticking(true);
            if (player1.GetComponent<FixedJoint>() == null) {
                player1.AddComponent<FixedJoint>();
            }
            player1.GetComponent<FixedJoint>().connectedBody = GameObject.Find("MazeBall").GetComponent<Rigidbody>();
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
        }  else if (Input.GetButton(p2StickButton) && p2CanPull) {
            p2Behaviour.SetIsPulling(true);
            player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            p2Behaviour.SetIsSticking(true);
            if (player2.GetComponent<FixedJoint>() == null) {
                player2.AddComponent<FixedJoint>();
            }
            player2.GetComponent<FixedJoint>().connectedBody = GameObject.Find("MazeBall").GetComponent<Rigidbody>();
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

        player1.transform.Find("Canvas").gameObject.transform.Find("Stick").gameObject.GetComponent<Image>().enabled = false;
        player1.transform.Find("Canvas").gameObject.transform.Find("StickPressed").gameObject.GetComponent<Image>().enabled = false;
        player2.transform.Find("Canvas").gameObject.transform.Find("Stick").gameObject.GetComponent<Image>().enabled = false;
        player2.transform.Find("Canvas").gameObject.transform.Find("StickPressed").gameObject.GetComponent<Image>().enabled = false;
        player1.transform.Find("Canvas").gameObject.transform.Find("Jump").gameObject.GetComponent<Image>().enabled = false;
        player2.transform.Find("Canvas").gameObject.transform.Find("Jump").gameObject.GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("MembraneSupportSphere").transform.Find("Canvas").gameObject.transform.Find("Seperate").gameObject.GetComponent<Text>().enabled = false;
        GameObject.FindWithTag("MembraneSupportSphere").transform.Find("Canvas").gameObject.transform.Find("StickTutorialText").gameObject.GetComponent<Text>().enabled = false;

        Vector3 player1positionNoY = player1position;
        Vector3 player2positionNoY = player2position;
        player1positionNoY.y = 0;
        player2positionNoY.y = 0;

        playerDistance = Vector3.Distance(player1positionNoY, player2positionNoY);

        if (p1CanStick) {
            if (p1Behaviour.GetIsSticking()) {
                player1.transform.Find("Canvas").gameObject.transform.Find("StickPressed").gameObject.GetComponent<Image>().enabled = true;
                if ((Vector3.Distance(player1position, player2position) > (maxSeparation - 0.6)) && !p2CanStick){
                    player2.transform.Find("Canvas").gameObject.transform.Find("Jump").gameObject.GetComponent<Image>().enabled = true;
                }
            } else {
                player1.transform.Find("Canvas").gameObject.transform.Find("Stick").gameObject.GetComponent<Image>().enabled = true;
            }           
        }

        if (p2CanStick) {
            if (p2Behaviour.GetIsSticking()) {
                player2.transform.Find("Canvas").gameObject.transform.Find("StickPressed").gameObject.GetComponent<Image>().enabled = true;
                if ((Vector3.Distance(player1position, player2position) > (maxSeparation - 0.6))  && !p1CanStick){
                    player1.transform.Find("Canvas").gameObject.transform.Find("Jump").gameObject.GetComponent<Image>().enabled = true;
                }
            } else {
                player2.transform.Find("Canvas").gameObject.transform.Find("Stick").gameObject.GetComponent<Image>().enabled = true;
            } 
        }


        
        Vector3 avg = (player1.transform.position + player2.transform.position) / 2;

        bool sticking = p1Behaviour.GetIsSticking() || p2Behaviour.GetIsSticking();
        bool pulling = p1Behaviour.GetIsPulling() || p2Behaviour.GetIsPulling();

        if (p1Grounded || p2Grounded || sticking) {
            player1.GetComponent<SpringJoint>().maxDistance = GameManager.maxSpringDistance;
        }

        if (!p1Grounded || !p2Grounded) {
            GameObject.FindWithTag("MembraneSupportSphere").transform.Find("SlimeTrail").GetComponent<ParticleSystem>().Play();
        }

        if(p1Grounded && p2Grounded) {
            squint = false;
            squinting();
        }

        if (p1Behaviour.GetIsGrounded()) {
            if (p1Behaviour.GetGroundedHit().transform.CompareTag("Jump")){
                if ((playerDistance> (maxSeparation - 0.6))) {
                    player1.transform.Find("Canvas").gameObject.transform.Find("Jump").gameObject.GetComponent<Image>().enabled = true;
                } else {
                    GameObject.FindWithTag("MembraneSupportSphere").transform.Find("Canvas").gameObject.transform.Find("Seperate").gameObject.GetComponent<Text>().enabled = true;
                }     
            }

            if (p1Behaviour.GetGroundedHit().transform.CompareTag("StickTutorial")){
                GameObject.FindWithTag("MembraneSupportSphere").transform.Find("Canvas").gameObject.transform.Find("StickTutorialText").gameObject.GetComponent<Text>().enabled = true;
            }
            if (p1Behaviour.GetGroundedHit().transform.CompareTag("dank_enable")){
                GameObject.FindWithTag("dank_canvas").GetComponent<Canvas>().enabled = true;
                GameObject.FindWithTag("dank_enable").GetComponent<AudioSource>().enabled = true;
                GameObject[] danks = GameObject.FindGameObjectsWithTag("dank");
                foreach (GameObject dank in danks)
                {
                    dank.GetComponent<MeshRenderer>().enabled = true;
                }
                GameObject[] not_danks = GameObject.FindGameObjectsWithTag("not_dank");
                foreach (GameObject not_dank in not_danks)
                {
                    not_dank.SetActive(false);
                }
                GameObject.FindWithTag("dank_enable").GetComponent<AudioSource>().Play();
                GameObject.FindWithTag("p1_iris_left").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindWithTag("p1_iris_right").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindWithTag("p2_iris_left").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindWithTag("p2_iris_right").GetComponent<MeshRenderer>().enabled = false;
                isDanking = true;
                squinting();
            }
            if (p1Behaviour.GetGroundedHit().transform.CompareTag("GameEnd")){
                SceneManager.LoadScene("End_Screen");
            }
        } 

        if (p2Behaviour.GetIsGrounded()) {
            if (p2Behaviour.GetGroundedHit().transform.CompareTag("Jump")){
                if ((playerDistance> (maxSeparation - 0.6))) {
                    player2.transform.Find("Canvas").gameObject.transform.Find("Jump").gameObject.GetComponent<Image>().enabled = true;
                } else {
                    GameObject.FindWithTag("MembraneSupportSphere").transform.Find("Canvas").gameObject.transform.Find("Seperate").gameObject.GetComponent<Text>().enabled = true;
                }
            }
            if (p2Behaviour.GetGroundedHit().transform.CompareTag("StickTutorial")){
                GameObject.FindWithTag("MembraneSupportSphere").transform.Find("Canvas").gameObject.transform.Find("StickTutorialText").gameObject.GetComponent<Text>().enabled = true;
            }
            if (p2Behaviour.GetGroundedHit().transform.CompareTag("dank_enable")){
                GameObject.FindWithTag("dank_canvas").GetComponent<Canvas>().enabled = true;
                GameObject.FindWithTag("dank_enable").GetComponent<AudioSource>().enabled = true;
                GameObject[] danks = GameObject.FindGameObjectsWithTag("dank");
                foreach (GameObject dank in danks)
                {
                    dank.GetComponent<MeshRenderer>().enabled = true;
                }
                GameObject[] not_danks = GameObject.FindGameObjectsWithTag("not_dank");
                foreach (GameObject not_dank in not_danks)
                {
                    not_dank.SetActive(false);
                }
                GameObject.FindWithTag("dank_enable").GetComponent<AudioSource>().Play();
                GameObject.FindWithTag("dank_enable").GetComponent<AudioSource>().Play();
                GameObject.FindWithTag("p1_iris_left").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindWithTag("p1_iris_right").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindWithTag("p2_iris_left").GetComponent<MeshRenderer>().enabled = false;
                GameObject.FindWithTag("p2_iris_right").GetComponent<MeshRenderer>().enabled = false;
                isDanking = true;
                squinting();
            }
            if (p2Behaviour.GetGroundedHit().transform.CompareTag("GameEnd")){
                SceneManager.LoadScene("End_Screen");
            }
        }

        player1.GetComponent<SpringJoint>().enableCollision = !(p1Behaviour.GetIsSticking() || p2Behaviour.GetIsSticking());

        // Players are max seperated, or in the air
        if (playerDistance > (maxSeparation - 0.6) || !p1Grounded || !p2Grounded)
        {   
            // Players are trying to jump
            if (p1AbleToJump || p2AbleToJump){

                // Players aren't sticking or pulling, and at least one of them is on the ground
                if (!sticking && !pulling && (p1Grounded || p2Grounded)) {

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
                    SoundEffectController.instance.Jump(); 
                    player1.GetComponent<SpringJoint>().maxDistance = 0;
                    squint = true;
                    squinting();
                } else if (p1Behaviour.GetIsSticking() && !pulling){//&& playerDistance > maxSeparation) {
                    Vector3 pull = avg;
                    pull = pull  - player2.transform.position;
                    pull.x = pull.x /2;
                    pull.z = pull.z /2;
                    pull.y += 0.1f;
                    player2.GetComponent<Rigidbody>().AddForce((pull).normalized * stickingJumpMagnitude);
                    player1.GetComponent<Rigidbody>().AddForce((pull).normalized * stickingJumpMagnitude);
                    //player1.GetComponent<SpringJoint>().enableCollision = false;
                    //player1.GetComponent<SpringJoint>().maxDistance = 0;
                    //player1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                } else if (p2Behaviour.GetIsSticking() && !pulling){//&& (playerDistance > (maxSeparation - 0.5)) {
                    Vector3 pull = avg;
                    pull = pull  - player1.transform.position;
                    pull.x = pull.x /2;
                    pull.z = pull.z /2;
                    pull.y += 0.1f;
                    player1.GetComponent<Rigidbody>().AddForce((pull).normalized * stickingJumpMagnitude);
                    player2.GetComponent<Rigidbody>().AddForce((pull).normalized * stickingJumpMagnitude);
                    //player1.GetComponent<SpringJoint>().enableCollision = false;
                    //player1.GetComponent<SpringJoint>().maxDistance = 0;
                    //player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                }/*  else if (p1Behaviour.GetIsPulling()){
                    player2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    Debug.Log("lol");
                    player1.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * stickingJumpMagnitude);
                }*/
                /* else if (p1Behaviour.GetIsSticking() && playerDistance > maxSeparation) {
                    player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * 20 * topSpeed);
                } else if (p2Behaviour.GetIsSticking() && playerDistance > maxSeparation) {
                    player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * 20 * topSpeed);
                }*/

                p1AbleToJump = false;
                p2AbleToJump = false;
            } else if (playerDistance > maxSeparation) {
                //player2.GetComponent<Rigidbody>().AddForce((avg - player2.transform.position).normalized * 20 * topSpeed);
                //player1.GetComponent<Rigidbody>().AddForce((avg - player1.transform.position).normalized * 20 * topSpeed);
            }

        }
    }
}
