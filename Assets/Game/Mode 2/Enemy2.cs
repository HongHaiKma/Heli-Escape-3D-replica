using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2 : MonoBehaviour, IDamageable
{
    public Transform tf_Owner;
    public bool isActive;
    public GameObject go_Warning;
    public StateMachine<Enemy2> m_StateMachine;
    public Animator m_Anim;
    public Transform tf_FirePivot;
    public EnemyState2 m_EnemyState;
    public Rigidbody rb_Owner;

    [Header("Charge Time")] 
    public float m_ReadyToShoot;
    public float m_ReadyToShootMax;

    private void OnEnable()
    {
        isActive = false;
        go_Warning.SetActive(false);
        m_ReadyToShoot = 0f;

        m_StateMachine = new StateMachine<Enemy2>(this);
        m_StateMachine.Init(IdleState2.Instance);
    }

    private void Update()
    {
        m_StateMachine.ExecuteStateUpdate();
    }

    public virtual void OnIdleEnter()
    {
        m_EnemyState = EnemyState2.Idle;
        m_Anim.SetTrigger("Idle");
    }
    
    public virtual void OnIdleExecute()
    {
        
    }
    
    public virtual void OnIdleExit()
    {
        
    }

    public virtual void OnAimEnter()
    {
        m_EnemyState = EnemyState2.Aim;
        m_Anim.SetTrigger("Aim");
        m_ReadyToShoot = 0f;
    }
    
    public virtual void OnAimExecute()
    {
        // tf_Owner.LookAt(new Vector3(0f, LevelController2.Instance.tf_Shooter.position.y, 0f));
        m_ReadyToShoot += Time.deltaTime;
        if (m_ReadyToShoot > m_ReadyToShootMax)
        {
            PrefabManager.Instance.SpawnBulletPool("BulletEnemy1", tf_FirePivot.position).GetComponent<Bullet>().Fire(LevelController2.Instance.tf_Shooter.position
                , LevelController2.Instance.tf_Shooter);
            ChangeState(WinState2.Instance);
        }
    }
    
    public virtual void OnAimExit()
    {
        
    }
    
    public virtual void OnWinEnter()
    {
        m_EnemyState = EnemyState2.Win;
    }
    
    public virtual void OnWinExecute()
    {

    }
    
    public virtual void OnWinExit()
    {
        
    }
    
    public void ChangeState(IState<Enemy2> state)
    {
        m_StateMachine.ChangeState(state);
    }
    
    public bool IsState(IState<Enemy2> state)
    {
        return m_StateMachine.curState == state;
    }

    public void SetInRange(bool _inRange)
    {
        if (_inRange)
        {
            if (!IsState(AimState.Instance))
            {
                ChangeState(AimState.Instance); 
            }
            
            if (!go_Warning.activeInHierarchy)
            {
                go_Warning.SetActive(true);
            }
        }
        else
        {
            if (!IsState(IdleState2.Instance))
            {
                ChangeState(IdleState2.Instance); 
            }

            if (go_Warning.activeInHierarchy)
            {
                go_Warning.SetActive(false);
            }
        }
    }
    
    public void DoRagdoll(Vector3 explosionPos)
    {
        rb_Owner.AddExplosionForce(2000f, explosionPos, 10f, 2f);
    }

    public void OnHit(Vector3 _pos)
    {
        PrefabManager.Instance.SpawnVFXPool("VFX_4", _pos);
        CamController2.Instance.m_Enemies.Remove(this);
        LevelController2.Instance.RemoveEnemy(this);
        PrefabManager.Instance.DespawnPool(gameObject);
    }
}

public enum EnemyState2
{
    Idle = 0, 
    Aim = 1,
    Win = 2,
}