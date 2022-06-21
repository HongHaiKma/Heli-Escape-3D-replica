using System;
using System.Collections;
using System.Collections.Generic;
using DynamicMeshCutter;
using Exploder;
using Exploder.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class BodyPart3 : MonoBehaviour, IBodyPart
{
    public Enemy3 m_EnemyOwner;
    public BodyParts3 m_BodyPart;
    public PlaneBehaviour m_PlaneCut;
    public bool m_Destroy;
    public Collider col_Owner;

    private void OnEnable()
    {
        m_Destroy = false;
        col_Owner.enabled = true;
    }

    public void OnHit()
    {
        m_EnemyOwner.m_Health -= 1f;

        if (m_EnemyOwner.m_Health <= 0f)
        {
            LevelController3.Instance.go_TrackPad.SetActive(false);
        }
        
        m_EnemyOwner.m_UIHealth.UpdateHealth();
        
        if (m_BodyPart == BodyParts3.HEAD)
        {
            m_EnemyOwner.m_HeadCut = true;

            m_Destroy = true;
            if (m_Destroy)
            {
                col_Owner.enabled = false;
                m_PlaneCut.Cut();
            }
        }
        else if (m_BodyPart == BodyParts3.L_UPPERLEG 
                 || m_BodyPart == BodyParts3.L_LOWERLEG 
                 || m_BodyPart == BodyParts3.L_FOOT)
        {
            m_EnemyOwner.m_FallBack = false;
            
            m_Destroy = true;
            if (m_Destroy)
            {
                if (m_EnemyOwner.m_LegCut == LegCut.NONE)
                {
                    col_Owner.enabled = false;
                    m_EnemyOwner.m_LegCut = LegCut.LEFT;
                    m_PlaneCut.Cut();
                }
            }
        }
        else if (m_BodyPart == BodyParts3.R_UPPERLEG 
                 || m_BodyPart == BodyParts3.R_LOWERLEG 
                 || m_BodyPart == BodyParts3.R_FOOT)
        {
            m_EnemyOwner.m_FallBack = false;
                
            m_Destroy = true;
            if (m_Destroy)
            {
                if (m_EnemyOwner.m_LegCut == LegCut.NONE)
                {
                    col_Owner.enabled = false;
                    m_EnemyOwner.m_LegCut = LegCut.RIGHT;
                    m_PlaneCut.Cut();
                }
            }
        }
        else if (m_BodyPart == BodyParts3.L_UPPERARM 
                 || m_BodyPart == BodyParts3.L_FOREARM)
        {
            m_EnemyOwner.m_FallBack = true;
            m_EnemyOwner.m_ArmCut = ArmCut.LEFT;
            
            m_Destroy = true;
            if (m_Destroy)
            {
                col_Owner.enabled = false;
                m_PlaneCut.Cut();
            }
        }
        else if (m_BodyPart == BodyParts3.R_UPPERARM 
                 || m_BodyPart == BodyParts3.R_FOREARM)
        {
            m_EnemyOwner.m_FallBack = true;
            m_EnemyOwner.m_ArmCut = ArmCut.RIGHT;
            
            m_Destroy = true;
            if (m_Destroy)
            {
                col_Owner.enabled = false;
                m_PlaneCut.Cut();
            }
        }
        else if(m_BodyPart == BodyParts3.BODY)
        {
            m_EnemyOwner.m_FallBack = true;
        }
        
        ChangeState(FallState3.Instance);
    }

    public bool OnCanSlowmotion()
    {
        return (m_EnemyOwner.m_Health - 4 <= 0);
    }
    
    public void ChangeState(IState<Enemy3> state)
    {
        m_EnemyOwner.ChangeState(state);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Construct"))
        {
            ExploderObject expld = col.GetComponent<ExploderObject>();
            if(expld != null) expld.ExplodeObject(gameObject);
            
            // ExploderSingleton.Instance.ExplodeObject(col.gameObject);
        }
    }
}

public enum BodyParts3
{
    HEAD,
    BODY,
    L_UPPERARM,
    L_FOREARM,
    R_UPPERARM,
    R_FOREARM,
    L_UPPERLEG,
    L_LOWERLEG,
    L_FOOT,
    R_UPPERLEG,
    R_LOWERLEG,
    R_FOOT,
}

public enum LegCut
{
    NONE,
    LEFT,
    RIGHT,
}

public enum ArmCut
{
    NONE,
    LEFT,
    RIGHT,
}