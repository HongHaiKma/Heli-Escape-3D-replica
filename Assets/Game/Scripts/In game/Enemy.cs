using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;
// using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Micosmo.SensorToolkit;
using Unity.VisualScripting;
using Random = System.Random;
using UnityEditor;

public class Enemy : MonoBehaviour, IDamageable
{
    public bool m_Warning;
    public Transform tf_Owner;
    public Animator m_Anim;
    
    public StateMachine<Enemy> m_StateMachine;

    public Collider[] AllColliders;
    public Collider MainCollider;
    public Rigidbody[] AllRigidbodies;
    public Collider col_Owner;
    public Rigidbody rb_Owner;

    public EnemyState m_EnemyState;

    [Header("AI")] 
    public AIPath m_AIPath;
    public Hostage m_TargetHostage;
    public float m_TimeChangeTarget;
    public bool isClimbing;
    public RaySensor m_RaySensor;
    public GameObject go_RaySensor;

    [Header("Time")] 
    public float m_TimeCatch;
    public float m_TimeCatchMax;
    
    private void OnEnable()
    {
        m_AIPath.canMove = true;
        m_AIPath.isStopped = false;
        isClimbing = false;
        m_TimeChangeTarget = 0f;
        col_Owner.enabled = true;
        m_Warning = false;
        go_RaySensor.SetActive(true);
        
        m_StateMachine = new StateMachine<Enemy>(this);
        m_StateMachine.Init(IdleState.Instance);
        
        EventManager.AddListener(GameEvent.LEVEL_LOSE, OnEnemyWin);
        EventManager.AddListener(GameEvent.LEVEL_WIN, OnEnemyLose);
        EventManager.AddListener(GameEvent.DespawnAllPool, DestroyAllPool);
        // m_RaySensor.OnDetected.AddListener(OnClimbStart());
    }

    private void OnDisable()
    {
        // if (m_Warning)
        // {
        //     m_Warning = false;
        //     g_Warning.SetActive(false);
        //     GameManager.Instance.SetSlowmotion(false);
        // }
        // m_StateMachine.ChangeState(IdleState.Instance);
        EventManager.RemoveListener(GameEvent.LEVEL_LOSE, OnEnemyWin);
        EventManager.RemoveListener(GameEvent.LEVEL_WIN, OnEnemyLose);
        EventManager.RemoveListener(GameEvent.DespawnAllPool, DestroyAllPool);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(GameEvent.LEVEL_LOSE, OnEnemyWin);
        EventManager.RemoveListener(GameEvent.LEVEL_WIN, OnEnemyLose);
        EventManager.RemoveListener(GameEvent.DespawnAllPool, DestroyAllPool);
    }

    private void Update()
    {
        m_StateMachine.ExecuteStateUpdate();
    }

    public void DestroyAllPool()
    {
        PrefabManager.Instance.DespawnPool(gameObject);
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
        m_AIPath.canMove = false;
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
    
    public virtual async UniTask OnChaseEnter()
    {
        m_EnemyState = EnemyState.Chase;
        m_RaySensor.enabled = true;
        m_AIPath.canMove = true;
        m_AIPath.isStopped = false;
        m_Anim.SetTrigger("Chase");
        m_TimeCatch = 0f;
    }
    
    public virtual async UniTask OnChaseExecute()
    {
        m_TimeChangeTarget += Time.deltaTime;

        if (LevelController.Instance.m_HostageRun.Count <= 0)
        {
            ChangeState(WinState.Instance);
            return;
        }
        else
        {
            if (m_TargetHostage == null)
            {
                m_TargetHostage = LevelController.Instance.m_HostageRun[UnityEngine.Random.Range(0, LevelController.Instance.m_HostageRun.Count - 1)];

                while (!LevelController.Instance.m_HostageRun.Contains(m_TargetHostage) 
                       || !m_TargetHostage.gameObject.activeInHierarchy 
                       || m_TargetHostage.m_HostageStates == HostageStates.DEATH)
                {
                    if (LevelController.Instance.m_HostageRun.Count <= 0)
                    {
                        ChangeState(WinState.Instance);
                        return;
                    }
                    m_TargetHostage = LevelController.Instance.m_HostageRun[UnityEngine.Random.Range(0, LevelController.Instance.m_HostageRun.Count - 1)];
                    await UniTask.Yield();
                }
            }
            else
            {
                while (!LevelController.Instance.m_HostageRun.Contains(m_TargetHostage) 
                       || !m_TargetHostage.gameObject.activeInHierarchy 
                       || m_TargetHostage.m_HostageStates == HostageStates.DEATH)
                {
                    if (LevelController.Instance.m_HostageRun.Count <= 0)
                    {
                        ChangeState(WinState.Instance);
                        return;
                    }
                    m_TargetHostage = LevelController.Instance.m_HostageRun[UnityEngine.Random.Range(0, LevelController.Instance.m_HostageRun.Count - 1)];
                    await UniTask.Yield();
                }
            }
        
            if (m_TimeChangeTarget > 5f)
            {
                m_TimeChangeTarget = 0f;
            
                m_TargetHostage = LevelController.Instance.m_HostageRun[UnityEngine.Random.Range(0, LevelController.Instance.m_HostageRun.Count - 1)];

                while (!LevelController.Instance.m_HostageRun.Contains(m_TargetHostage) 
                       || !m_TargetHostage.gameObject.activeInHierarchy 
                       || m_TargetHostage.m_HostageStates == HostageStates.DEATH)
                {
                    if (LevelController.Instance.m_HostageRun.Count <= 0)
                    {
                        ChangeState(WinState.Instance);
                        return;
                    }
                    m_TargetHostage = LevelController.Instance.m_HostageRun[UnityEngine.Random.Range(0, LevelController.Instance.m_HostageRun.Count - 1)];
                    await UniTask.Yield();
                }
            } 
        }

        
        
        m_AIPath.destination = m_TargetHostage.tf_Onwer.position;

        // if(m_Warning) m_TimeCatch += Time.deltaTime;
        
        // if (m_TimeCatch > m_TimeCatchMax)
        // {
        //     ChangeState(KillState.Instance);
        //     // EventManager.CallEvent(GameEvent.LEVEL_LOSE);
        //     return;
        // }

        if (Helper.CalDistance(tf_Owner.position, m_TargetHostage.tf_Onwer.position) < 0.5f)
        {
            m_StateMachine.ChangeState(KillState.Instance);
            // EventManager.CallEvent(GameEvent.LEVEL_LOSE);
            // return;
        }
        
        // if (Helper.CalDistance(tf_Owner.position, m_TargetHostage.tf_Onwer.position) < 6f)
        //     // if (m_AIPath.remainingDistance < 3f)
        // {
        //     if (!m_Warning)
        //     {
        //         m_Warning = true;
        //         g_Warning.SetActive(true);
        //         GameManager.Instance.SetSlowmotion(true);
        //     }
        // }
        // else
        // {
        //     if (m_Warning)
        //     {
        //         m_Warning = false;
        //         g_Warning.SetActive(false);
        //         GameManager.Instance.SetSlowmotion(false);
        //     }
        // }
    }

    public void SetSlowmotionOff()
    {
        m_Warning = false;
        // g_Warning.SetActive(false);
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
        go_RaySensor.SetActive(false);
        // DoRagdoll(tf_Owner.position);
        // if (m_Warning)
        // {
        //     m_Warning = false;
        //     g_Warning.SetActive(false);
        //     GameManager.Instance.SetSlowmotion(false);  
        // }

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
        // m_Anim.SetTrigger("Kill");
        // m_AIPath.destination = tf_Owner.position;
        // m_AIPath.canMove = false;
        // tf_Owner.position = tf_Owner.position + Vector3.up * 4;
        m_TargetHostage.ChangeState(P_DeathState.Instance);
        ChangeState(ChaseState.Instance);
    }

    public void OnEnemyWin()
    {
        ChangeState(WinState.Instance);
        // SetSlowmotionOff();
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

    public virtual void OnClimbEnter()
    {
        m_EnemyState = EnemyState.Climb;
        rb_Owner.useGravity = false;
        rb_Owner.isKinematic = true;
        m_Anim.SetTrigger("Climb");
        m_AIPath.canMove = false;
        m_AIPath.isStopped = true;
    }
    
    public virtual void OnClimbExecute()
    {
        rb_Owner.position += Vector3.up * 2f * Time.deltaTime;
        // Helper.DebugLog("CLIMBBBBBBBBBBBBBBB");
    }
    
    public virtual void OnClimbExit()
    {
        // rb_Owner.position = rb_Owner.position + new Vector3(rb_Owner.transform.forward.normalized ) rb_Owner.transform.forward.normalized * 2f;
        
        rb_Owner.useGravity = true;
        rb_Owner.isKinematic = false;
        m_RaySensor.enabled = false;
        m_AIPath.canMove = true;
        m_AIPath.isStopped = false;
    }
    
    public virtual void OnWinEnter()
    {
        m_EnemyState = EnemyState.Win;
        m_Anim.SetTrigger("OnHostageWin");
    }
    
    public virtual void OnWinExecute()
    {
        
    }
    
    public virtual void OnWinExit()
    {
        
    }

    public void OnClimbStart()
    {
        Helper.DebugLog("OnClimbStartOnClimbStartOnClimbStartOnClimbStart");
        
        if (!isClimbing)
        {
            isClimbing = true;
            ChangeState(ClimbState.Instance); 
        }
    }
    
    public void OnClimbEnd()
    {
        // Helper.DebugLog("Endddddddddddddddddddddddddddddddd");
        isClimbing = false;
        go_RaySensor.SetActive(false);
        
        GraphNode nearestNode = AstarPath.active.GetNearest(tf_Owner.position, NNConstraint.Default).node;
        if (nearestNode != null)
        {
            Vector3 aaa = nearestNode.ClosestPointOnNode(tf_Owner.position);
            
            tf_Owner.DOMove(aaa, 1.5f).OnComplete(
                () =>
                {
                    ChangeState(ChaseState.Instance);
                    tf_Owner.position = aaa;
                });
            // Helper.DebugLog("Closet point: " + aaa);
            // Helper.DebugLog("STANDDDDDDDDDDDDDDDDDDDD");
            // EditorApplication.isPaused = true;
        }

        // await UniTask.Delay(1000);
        
    }

    public void ChangeState(IState<Enemy> state)
    {
        m_StateMachine.ChangeState(state);
    }

    public void OnHit(Vector3 _pos)
    {
        col_Owner.enabled = false;
        
        InGameManager.Instance.Combo();
        
        ChangeState(DeathState.Instance);
                
        PrefabManager.Instance.SpawnVFXPool("VFX_4", _pos);
        PrefabManager.Instance.SpawnVFXPool("UIDamage", Vector3.zero).GetComponent<UIDamage>().Fly(_pos);
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
    Climb = 4,
    Win = 5,
}
