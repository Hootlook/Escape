using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerController pc;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        pc = GetComponent<PlayerController>();
    }


    void Update()
    {
        if (CameraManager.instance.isLocking && !pc.isRunning)
        {
            anim.SetBool("lockon", true);
        }
        else
        {
            anim.SetBool("lockon", false);
        }


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

        anim.SetFloat("vertical", pc.stateSpeed.y, 0.4f, Time.deltaTime);
        anim.SetFloat("horizontal", pc.stateSpeed.x, 0.4f, Time.deltaTime);
        anim.SetFloat("magnitude", pc.stateSpeed.magnitude, 0.2f, Time.deltaTime);

    }
}