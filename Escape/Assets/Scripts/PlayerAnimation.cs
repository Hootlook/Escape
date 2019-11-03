using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    CharacterController controller;
    PlayerController pc;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        pc = GetComponent<PlayerController>();
    }


    void Update()
    {
        if (pc.isRolling == true)
        {
            anim.SetBool("rolling", true);
        }
        else anim.SetBool("rolling", false);
        if (pc.isJumpingBack)
        {
            anim.SetBool("stepBack", true);
        }
        else anim.SetBool("stepBack", false);
        anim.SetFloat("vertical", pc.stateSpeed, 0.4f, Time.deltaTime);
    }
}