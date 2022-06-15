using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    private Animator m_Anim;
    private RagdollController ragdollController;

    private void Awake()
    {
        // m_Anim = GetComponent<Animator>();
        ragdollController = GetComponent<RagdollController>();
    }

    public void OnEnemyShot(Vector3 shootDirection, Rigidbody shotRB)
    {
        // StopAnimation();
        // ragdollController.EnableRagdoll();
        if (shotRB)
        {
            shotRB.AddForce(shootDirection.normalized * 100f, ForceMode.Impulse);
        }            
    }

    // public void StopAnimation()
    // {
    //     m_Anim.enabled = false;
    // }
}
