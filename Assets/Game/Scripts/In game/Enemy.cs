using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;
// using System.Threading;
using Cysharp.Threading.Tasks;

public class Enemy : MonoBehaviour, IDamageable
{
    public bool m_Warning;
    public Transform tf_Owner;
    public GameObject g_Warning;
    public Animator m_Anim;
    
    public StateMachine<Enemy> m_StateMachine;

    public Collider[] AllColliders;
    public Collider MainCollider;
    public Rigidbody[] AllRigidbodies;

    public EnemyState m_EnemyState;

    [Header("AI")] 
    public AIPath m_AIPath;

    [Header("Time")] 
    public float m_TimeCatch;
    public float m_TimeCatchMax;

    private void Awake()
    {
        AllColliders = GetComponentsInChildren<Collider>(true);
        AllRigidbodies = GetComponentsInChildren<Rigidbody>(true);
    }

    private void OnEnable()
    {
        // m_AIPath.canMove = false;
        m_Warning = false;
        g_Warning.SetActive(false);
        
        m_StateMachine = new StateMachine<Enemy>(this);
        m_StateMachine.Init(IdleState.Instance);
        m_EnemyState = EnemyState.Idle;
        // m_StateMachine.ChangeState(ChaseState.Instance);

        EventManager.AddListener(GameEvent.LEVEL_LOSE, OnEnemyKill);
        EventManager.AddListener(GameEvent.LEVEL_WIN, OnEnemyLose);
    }

    private void OnDisable()
    {
        if (m_Warning)
        {
            m_Warning = false;
            g_Warning.SetActive(false);
            GameManager.Instance.SetSlowmotion(false);
        }
        
        EventManager.RemoveListener(GameEvent.LEVEL_LOSE, OnEnemyKill);
        EventManager.RemoveListener(GameEvent.LEVEL_WIN, OnEnemyLose);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(GameEvent.LEVEL_LOSE, OnEnemyKill);
        EventManager.RemoveListener(GameEvent.LEVEL_WIN, OnEnemyLose);
    }

    private void Update()
    {
        m_StateMachine.ExecuteStateUpdate();
    }

    public void DoRagdoll(Vector3 explosionPos)
    {
        foreach (var col in AllColliders)
        {
            col.enabled = true;
        }

        m_Anim.enabled = false;
        GetComponent<CharacterController>().enabled = false;
        m_AIPath.enabled = false;

        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        foreach (var mrb in AllRigidbodies)
        {
            // mrb.useGravity = false;
            mrb.isKinematic = false;
        }
        
        // MainCollider.enabled = false;
        rb.AddExplosionForce(2000f, explosionPos, 10f, 2f);
        // rb.AddForce((explosionPos - tf_Owner.position).normalized * 10f);
        
    }

    public virtual void OnIdleEnter()
    {
        m_EnemyState = EnemyState.Idle;
        m_AIPath.destination = tf_Owner.position;
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
    
    public virtual void OnChaseEnter()
    {
        m_EnemyState = EnemyState.Chase;
        m_Anim.SetTrigger("Chase");
        m_TimeCatch = 0f;
    }
    
    public virtual void OnChaseExecute()
    {
        m_AIPath.destination = LevelController.Instance.m_Hostage.tf_Onwer.position;

        if(m_Warning) m_TimeCatch += Time.deltaTime;
        
        if (m_TimeCatch > m_TimeCatchMax)
        {
            ChangeState(KillState.Instance);
            // EventManager.CallEvent(GameEvent.LEVEL_LOSE);
            return;
        }

        // if (Helper.CalDistance(tf_Owner.position, LevelController.Instance.m_Hostage.tf_Onwer.position) < 3f)
        // {
        //     m_StateMachine.ChangeState(KillState.Instance);
        //     EventManager.CallEvent(GameEvent.LEVEL_LOSE);
        //     return;
        // }
        
        if (Helper.CalDistance(tf_Owner.position, LevelController.Instance.m_Hostage.tf_Onwer.position) < 6f)
            // if (m_AIPath.remainingDistance < 3f)
        {
            if (!m_Warning)
            {
                m_Warning = true;
                g_Warning.SetActive(true);
                GameManager.Instance.SetSlowmotion(true);
            }
        }
        else
        {
            if (m_Warning)
            {
                m_Warning = false;
                g_Warning.SetActive(false);
                GameManager.Instance.SetSlowmotion(false);
            }
        }
    }

    public void SetSlowmotionOff()
    {
        m_Warning = false;
        g_Warning.SetActive(false);
        GameManager.Instance.SetSlowmotion(false);
    }
    
    public virtual void OnChaseExit()
    {
        
    }
    
    public virtual async UniTask OnDeathEnter()
    {
        m_EnemyState = EnemyState.Death;
        m_Anim.SetTrigger("Death");
        // m_AIPath.destination = tf_Owner.position;
        m_AIPath.canMove = false;
        // DoRagdoll(tf_Owner.position);
        if (m_Warning)
        {
            m_Warning = false;
            g_Warning.SetActive(false);
            GameManager.Instance.SetSlowmotion(false);  
        }

        await UniTask.Delay(2000);
        
        PrefabManager.Instance.DespawnPool(gameObject);
    }
    
    public virtual void OnDeathExecute()
    {
        
    }
    
    public virtual void OnDeathExit()
    {
        
    }
    
    public virtual void OnKillEnter()
    {
        m_EnemyState = EnemyState.Kill;
        m_Anim.SetTrigger("Kill");
        // m_AIPath.destination = tf_Owner.position;
        m_AIPath.canMove = false;
        // tf_Owner.position = tf_Owner.position + Vector3.up * 4;
        LevelController.Instance.m_Hostage.Death();
    }

    public void OnEnemyKill()
    {
        ChangeState(KillState.Instance);
        SetSlowmotionOff();
    }
    
    public void OnEnemyLose()
    {
        ChangeState(IdleState.Instance);
    }
    
    public virtual void OnKillExecute()
    {
        
    }
    
    public virtual void OnKillExit()
    {
        
    }

    public void ChangeState(IState<Enemy> state)
    {
        m_StateMachine.ChangeState(state);
    }

    public void OnHit(Vector3 _pos)
    {
        InGameManager.Instance.Combo();
        
        ChangeState(DeathState.Instance);
                
        PrefabManager.Instance.SpawnVFXPool("VFX_4", _pos);
    }

    [Button]
    public void SetupRagdoll()
    {
        AllRigidbodies = GetComponentsInChildren<Rigidbody>(true);
        foreach (var mrb in AllRigidbodies)
        {
            mrb.useGravity = true;
            mrb.mass = 1f;
            mrb.angularDrag = 0f;
            mrb.drag = 0f;
            
            mrb.constraints = RigidbodyConstraints.None;
            // mrb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}

public enum EnemyState
{
    Idle = 0,
    Chase = 1,
    Death = 2,
    Kill = 3,
}
