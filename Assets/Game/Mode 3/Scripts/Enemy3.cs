using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public float m_Health;
    
    public Animator m_Anim;
    private RagdollController ragdollController;

    private StateMachine<Enemy3> m_StateMachine;
    public EnemyState3 m_EnemyState;

    private void Awake()
    {
        ragdollController = GetComponent<RagdollController>();
    }

    private void OnEnable()
    {
        // m_Health = 3f;
        m_StateMachine = new StateMachine<Enemy3>(this);
        m_StateMachine.Init(IdleState3.Instance);
    }
    
    public virtual void OnIdleEnter()
    {
        m_EnemyState = EnemyState3.Idle;
        // m_AIPath.canMove = false;
        m_Anim.SetTrigger("Idle");
        // m_AIPath.destination = GameManager.Instance.m_Hostage.tf_Owner.position;
        // m_AIPath.
    }
    
    public virtual void OnIdleExecute()
    {
        
    }
    
    public virtual void OnIdleExit()
    {
        
    }
    
    public void OnDeathEnter()
    {
        m_EnemyState = EnemyState3.Death;

        // skin_Owner.material.DOKill();
        // skin_Owner.material.DOColor(Color.black, "_Color", 1.5f);

        m_Anim.SetTrigger("Death");
        // m_AIPath.canMove = false;
        // go_RaySensor.SetActive(false);
    }
    
    public virtual void OnDeathExecute()
    {
        // f_TimeDeath += Time.deltaTime;
        // if (f_TimeDeath > 2f)
        // {
        //     // skin_Owner.material.SetColor("Color", new Color(209f, 38f, 49f, 255f));
        //     PrefabManager.Instance.DespawnPool(gameObject);
        // }
    }
    
    public virtual void OnDeathExit()
    {
        
    }
    
    // public void OnEnemyShot(Vector3 shootDirection, Rigidbody shotRB, bool isDie)
    public void OnEnemyShot(bool isDie)
    {
        // StopAnimation();
        // ragdollController.EnableRagdoll();
        if (isDie)
        {
            // shotRB.AddForce(shootDirection.normalized * 100f, ForceMode.Impulse);
            m_Anim.SetTrigger("Death");
        }
        else
        {
            m_Health -= 1f;
        }
    }

    // public void OnHit(Vector3 shootDirection, Rigidbody shotRB, bool isDie)
    public void OnHit(bool isDie)
    {
        // OnEnemyShot(shootDirection, shotRB, isDie);
        OnEnemyShot(isDie);
    }
}

public enum EnemyState3
{
    Idle,
    Chase,
    Attack,
    Death,
}