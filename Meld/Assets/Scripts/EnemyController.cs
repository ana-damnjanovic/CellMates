using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;
    public bool hostile = false;
    public float vision = 7f;
    public float idleDuration = 3f;
    public float wanderRadius = 10f;
    private float idleRemaining = 0f;
    private float movementTimeout = 2f;
    private Vector3 randomDestination = Vector3.zero;
    private NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent <NavMeshAgent> ();
        player = GameObject.FindWithTag("MembraneSupportSphere").transform;
        randomDestination = rb.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movementTimeout = System.Math.Max(movementTimeout - Time.deltaTime, 0);

        // Will idle
        if (Vector3.Distance(this.transform.position, randomDestination) < 3 || movementTimeout == 0)
        {
            if (idleRemaining > 0)
            {
                idleRemaining = System.Math.Max(idleRemaining - Time.deltaTime, 0);
                nav.isStopped = true; //nav.SetDestination(rb.transform.position);
            }
            if (idleRemaining == 0) {
                idleRemaining = idleDuration;
                randomDestination = RandomNavSphere(wanderRadius);
                nav.SetDestination(randomDestination);
                //nav.enabled = true;
                nav.isStopped = false;
                movementTimeout = idleDuration;
            }
        }
        else if (hostile && Vector3.Distance(this.transform.position, player.position) < vision)
        {
            nav.SetDestination(player.position);
        }
        //Debug.DrawRay (rb.transform.position, randomDestination, Color.red);
    }
    private Vector3 RandomNavSphere (float radius) {

            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
           
            randomDirection += rb.transform.position;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, radius, NavMesh.AllAreas);
           
            var temp = navHit.position;
            //temp.y = rb.transform.position.y;

            return temp;
    }
}
