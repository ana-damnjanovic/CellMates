using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager cm;
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
    }
    // called whenever another collider enters our zone (if layers match)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Players"))
        {
            cm.lastCheckpointPosition = transform.position;
        }
    }
}
