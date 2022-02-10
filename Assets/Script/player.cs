using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class player : MonoBehaviourPun
{
    private Animator anim;
    private Rigidbody rigid;

    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        if(!photonView.IsMine)
        {
            Destroy(GetComponent<player>());
        }
    }

    void Update()
    {
        var v  = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", v);
        anim.SetFloat("turningspeed", h);
    }

}
