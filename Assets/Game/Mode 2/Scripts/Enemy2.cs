using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2 : MonoBehaviour, IEnemy2
{
    public Transform tf_Owner;
    public bool isActive;
    public GameObject go_Warning;
    public StateMachine<Enemy2> m_StateMachine;
    public Animator m_Anim;
    public Transform tf_FirePivot;
    public EnemyState2 m_EnemyState;
    public Rigidbody rb_Owner;
    public float m_DetectDegree;

    [Header("Charge Time")] 
    public float m_ReadyToShoot;
    public float m_ReadyToShootMax;
    public float m_DeathTime;
    public float m_DeathTimeMax;

    private void OnEnable()
    {
        isActive = false;
        go_Warning.SetActive(false);
        m_ReadyToShoot = 0f;
        m_DeathTime = 0f;

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
        // LookShooter(2f);
        // LookShooter(2f);
    }
    
    public async UniTask LookShooter(float duration)
    {
        float time = 0;
        Vector3 startPosition = tf_Owner.position;
        Vector3 shooter = LevelController2.Instance.m_Shooter.transform.position;
        // Vector3 lookat = new Vector3(0f, shooter.y - tf_Owner.position.y ,0f);
        Quaternion lookat = new Quaternion(0f, shooter.y - tf_Owner.position.y ,0f, 0f);
        // Quaternion look = Quaternion.LookRotation(lookat, Vector3.up);
        while (time < duration)
        {
            // Vec = Vector3.Lerp(startPosition, look, time / duration);
            transform.rotation = Quaternion.Lerp(tf_Owner.rotation, lookat, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        tf_Owner.rotation = lookat;
        // UIIngame2.Instance.go_PopupLose.SetActive(true);
    }
    
    public virtual void OnAimExecute()
    {
        // LookShooter(2f);
        // tf_Owner.LookAt(new Vector3(0f, LevelController2.Instance.tf_Shooter.position.y, 0f));
        m_ReadyToShoot += Time.deltaTime;
        if (m_ReadyToShoot > m_ReadyToShootMax)
        {
            Vector3 shooterPos = LevelController2.Instance.m_Shooter.tf_TargetPoint.position;
            PrefabManager.Instance.SpawnBulletPool("BulletEnemy1", tf_FirePivot.position).GetComponent<Bullet>().Fire(shooterPos
                , LevelController2.Instance.m_Shooter.tf_TargetPoint);
            ChangeState(WinState2.Instance);
        }
        // else
        // {
        //     Vector3 shooter = LevelController2.Instance.m_Shooter.transform.position;
        //     // Quaternion lookat = new Quaternion(0f, shooter.y - tf_Owner.position.y ,0f, 0f);
        //     Vector3 lookat = new Vector3(0f, shooter.y - tf_Owner.position.y ,0f);
        //     tf_Owner.LookAt(lookat);
        //     Helper.DebugLog("KKKKKKKKKKKK");
        // }
    }
    
    public virtual void OnAimExit()
    {
        
    }
    
    public virtual void OnDeathEnter()
    {
        m_EnemyState = EnemyState2.Death;
        m_Anim.SetTrigger("Death");
        
        if (go_Warning.activeInHierarchy)
        {
            go_Warning.SetActive(false);
        }
    }
    
    public virtual void OnDeathExecute()
    {
        m_DeathTime += Time.deltaTime;
        if (m_DeathTime >= m_DeathTimeMax)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
        }
    }
    
    public virtual void OnDeathExit()
    {
        
    }
    
    public virtual void OnWinEnter()
    {
        m_EnemyState = EnemyState2.Win;
        if (go_Warning.activeInHierarchy)
        {
            go_Warning.SetActive(false);
        }
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

    public async UniTask SetInRange(bool _inRange)
    {
        if (_inRange)
        {
            if (!IsState(AimState.Instance))
            {
                await UniTask.WaitUntil(() => this.isActiveAndEnabled == true);
                await UniTask.WaitForEndOfFrame();
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
        if (m_EnemyState != EnemyState2.Death)
        {
            PrefabManager.Instance.SpawnVFXPool("VFX_4", _pos);
            CamController2.Instance.m_Enemies.Remove(this);
            LevelController2.Instance.RemoveEnemy(this);
            ChangeState(DeathState2.Instance);
        }
    }
}

public enum EnemyState2
{
    Idle = 0, 
    Aim = 1,
    Win = 2,
    Death = 3,
}