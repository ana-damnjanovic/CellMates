using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // To use this script:
    // 1) Add it as a component of the player model 
    // 2) Drag the player (its parent) into the player field
    private Animator animator;
    public GameObject player;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (player.GetComponent<playerBehaviour>().GetIsSticking() || player.GetComponent<playerBehaviour>().GetIsPulling())
        {
            animator.SetBool("isSticking", true);
        } 
        else
        {
            animator.SetBool("isSticking", false);
        }
    }
}
