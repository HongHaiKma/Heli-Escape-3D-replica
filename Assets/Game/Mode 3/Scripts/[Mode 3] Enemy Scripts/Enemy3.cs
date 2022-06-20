using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    protected static readonly int animIdle = Animator.StringToHash("Idle");
    protected static readonly int animAttack = Animator.StringToHash("Attack");
    protected static readonly int animAttack2 = Animator.StringToHash("Attack2");
    protected static readonly int animDeath = Animator.StringToHash("Death");
    protected static readonly int animChase = Animator.StringToHash("Chase");
    protected static readonly int animFallBack = Animator.StringToHash("Fall_Back");
    protected static readonly int animFall = Animator.StringToHash("Fall");
    protected static readonly int animChaseLLeg = Animator.StringToHash("ChaseLLeg");
    protected static readonly int animChaseRLeg = Animator.StringToHash("ChaseRLeg");
    protected static readonly int animChaseLArm = Animator.StringToHash("ChaseLArm");
    protected static readonly int animChaseRArm = Animator.StringToHash("ChaseRArm");

    [Title("Attributes")]
    public float m_Health;
    
    public Animator m_Anim;
    private RagdollController ragdollController;
    public UIEnemy3Bar m_UIHealth;
    public float m_AnimSpd;

    [Title("States")]
    public LegCut m_LegCut;
    public ArmCut m_ArmCut;
    public bool m_HeadCut;
    public bool m_FallBack;
    private StateMachine<Enemy3> m_StateMachine;
    public EnemyState3 m_EnemyState;
    public Hostage3 m_HostageTarget;
    
    [Title("Attack Area")]
    public Vector3 m_ExplosionRadius;
    public Transform tf_ExploPivot;

    [Title("AI")] 
    public AIPath m_AI;

    private void Awake()
    {
        ragdollController = GetComponent<RagdollController>();
    }

    private void OnEnable()
    {
        // m_Health = 3f;
        m_Anim.speed = m_AnimSpd;
        m_StateMachine = new StateMachine<Enemy3>(this);
        m_StateMachine.Init(ChaseState3.Instance);

        m_LegCut = LegCut.NONE;
        m_ArmCut = ArmCut.NONE;
        m_HeadCut = false;
    }

    private void Update()
    {
        m_StateMachine.ExecuteStateUpdate();
    }

    #region StateMachine

    [Button]
    public void TestDestination()
    {
        m_AI.destination = m_HostageTarget.tf_Owner.position;
    }
    
    public virtual void OnIdleEnter()
    {
        m_EnemyState = EnemyState3.Idle;
        // m_AIPath.canMove = false;
        m_Anim.SetTrigger(animIdle);
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
        m_EnemyState = EnemyState3.Chase;
        Helper.DebugLog("Chaseeeeeeeeeeeeeeee");
        // m_AIPath.canMove = false;
        if (m_LegCut == LegCut.NONE)
        {
            if (m_ArmCut == ArmCut.NONE)
                m_Anim.SetTrigger(animChase);
            else if (m_ArmCut == ArmCut.LEFT)
                m_Anim.SetTrigger(animChaseRArm);
            else if (m_ArmCut == ArmCut.RIGHT) 
                m_Anim.SetTrigger(animChaseLArm);
        }
        else if (m_LegCut == LegCut.LEFT)
        {
            Helper.DebugLog("Chase R Leg");
            m_Anim.SetTrigger(animChaseRLeg);
        }
        else if (m_LegCut == LegCut.RIGHT)
        {
            Helper.DebugLog("Chase L Leg");
            m_Anim.SetTrigger(animChaseLLeg);
        }

        if (m_HostageTarget == null)
        {
            List<Hostage3> a = LevelController3.Instance.m_Hostages;
            m_HostageTarget = a[UnityEngine.Random.Range(0, a.Count)];
        }
        
        // m_AIPath.destination = GameManager.Instance.m_Hostage.tf_Owner.position;
        // m_AIPath.
    }
    
    public virtual void OnChaseExecute()
    {
        if (m_HostageTarget == null)
        {
            List<Hostage3> a = LevelController3.Instance.m_Hostages;
            m_HostageTarget = a[UnityEngine.Random.Range(0, a.Count)];
        }

        m_AI.destination = m_HostageTarget.tf_Owner.position;

        if ((transform.position - m_HostageTarget.tf_Owner.position).sqrMagnitude < 0.5f)
        {
            ChangeState(AttackState3.Instance);
            Helper.DebugLog("Chase to Attack");
        }
    }
    
    public virtual void OnChaseExit()
    {
        
    }
    
    public virtual void OnAttackEnter()
    {
        m_EnemyState = EnemyState3.Attack;
        
        // m_AIPath.canMove = false;
        if (m_LegCut == LegCut.RIGHT)
        {
            m_Anim.ResetTrigger(animAttack2);
            m_Anim.SetTrigger(animAttack2);
            
            Helper.DebugLog("Attack Left");
        }
        else
        {
            m_Anim.ResetTrigger(animAttack);
            m_Anim.SetTrigger(animAttack);
           
            Helper.DebugLog("Attack Right");
        }

        // m_AIPath.destination = GameManager.Instance.m_Hostage.tf_Owner.position;
        // m_AIPath.
    }

    [Button]
    public void TestAttack()
    {
        // m_Anim.SetTrigger(animAttack);
        ChangeState(AttackState3.Instance);
    }
    
    [Button]
    public void TestOverLap()
    {
        Collider[] colliders = Physics.OverlapBox(tf_ExploPivot.position, m_ExplosionRadius);
        
        Helper.DebugLog("Count: " + colliders.Length);
        
        foreach (Collider hit in colliders)
        {
            Helper.DebugLog(hit.name);
            // Hostage3 hos = hit.GetComponent<Hostage3>();
            //
            // if (hos != null)
            // {
            //     hos.OnHit(Vector3.up);
            //     Helper.DebugLog("Enemy on Hit");
            // }
        }
    }

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapBox(tf_ExploPivot.position, m_ExplosionRadius);
        
        foreach (Collider hit in colliders)
        {
            IDamageable hos = hit.GetComponent<IDamageable>();

            if (hos != null)
            {
                hos.OnHit(Vector3.up);
                Helper.DebugLog("Enemy on Hit");
            }
        }
    }

    public virtual void OnAttackExecute()
    {
        
    }
    
    public virtual void OnAttackExit()
    {
        
    }
    
    public virtual void OnFallEnter()
    {
        m_EnemyState = EnemyState3.Fall;
        // m_AIPath.canMove = false;
        if (m_FallBack)
            m_Anim.SetTrigger(animFallBack);
        else 
            m_Anim.SetTrigger(animFall);
        
        // m_AIPath.destination = GameManager.Instance.m_Hostage.tf_Owner.position;
        // m_AIPath.
    }
    
    public virtual void OnFallExecute()
    {
        
    }
    
    public virtual void OnFallExit()
    {
        
    }
    
    public void OnDeathEnter()
    {
        m_EnemyState = EnemyState3.Death;

        // skin_Owner.material.DOKill();
        // skin_Owner.material.DOColor(Color.black, "_Color", 1.5f);

        m_Anim.SetTrigger(animDeath);
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
    
    public bool IsState(IState<Enemy3> state)
    {
        return m_StateMachine.curState == state;
    }
    
    public void ChangeState(IState<Enemy3> state)
    {
        m_StateMachine.ChangeState(state);
    }

    #endregion

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(tf_ExploPivot.position, m_ExplosionRadius);
    }
#endif
    
    // public void OnHit(Vector3 shootDirection, Rigidbody shotRB, bool isDie)
    public void OnHit(bool isDie)
    {
        // OnEnemyShot(shootDirection, shotRB, isDie);
        OnEnemyShot(isDie);
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
}

public enum EnemyState3
{
    Idle,
    Chase,
    Attack,
    Fall,
    Death,
}