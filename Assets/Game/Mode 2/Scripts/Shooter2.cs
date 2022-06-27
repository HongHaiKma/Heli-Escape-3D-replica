using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Shooter2 : MonoBehaviour, IDamageable
{
    public ShooterStates2 m_ShooterState;
    public StateMachine<Shooter2> m_StateMachine;
    public Animator m_Anim;
    public Transform tf_TargetPoint;

    private void OnEnable()
    {
        m_StateMachine = new StateMachine<Shooter2>(this);
        m_StateMachine.Init(P_AimState2.Instance);
    }
    
    public void ChangeState(IState<Shooter2> state)
    {
        m_StateMachine.ChangeState(state);
    }

    public bool IsState(IState<Shooter2> state)
    {
        return m_StateMachine.curState == state;
    }
    
    public virtual void OnAimEnter()
    {
        m_ShooterState = ShooterStates2.Aim;
        m_Anim.SetTrigger("Aim");
    }
    
    public virtual void OnAimExecute()
    {

    }
    
    public virtual void OnAimExit()
    {
        
    }
    
    public virtual void OnDeathEnter()
    {
        m_ShooterState = ShooterStates2.Death;
        m_Anim.SetTrigger("Death");
    }
    
    public virtual void OnDeathExecute()
    {

    }
    
    public virtual void OnDeathExit()
    {
        
    }
    
    public void OnHit(Vector3 _pos)
    {
        if (!IsState(P_DeathState2.Instance))
        {
            
            ChangeState(P_DeathState2.Instance);
            PrefabManager.Instance.SpawnVFXPool("VFX_4", _pos);

            Vector3 target = new Vector3(CamController2.Instance.m_CMCamOffset.m_Offset.x,
                CamController2.Instance.m_CMCamOffset.m_Offset.y, -2.8f);
            CamController2.Instance.CameraDeathAnimation(target, 1.5f);
            LevelController2.Instance.tf_PivotFollower.DOKill();
        }
    }
}

public enum ShooterStates2
{
    Aim = 0, 
    Death = 1,
}
