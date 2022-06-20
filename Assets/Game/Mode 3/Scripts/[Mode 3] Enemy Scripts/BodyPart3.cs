using System.Collections;
using System.Collections.Generic;
using DynamicMeshCutter;
using UnityEngine;

public class BodyPart3 : MonoBehaviour, IBodyPart
{
    public Enemy3 m_EnemyOwner;
    public BodyParts3 m_BodyPart;
    public PlaneBehaviour m_PlaneCut;

    public void OnHit()
    {
        if (m_BodyPart == BodyParts3.HEAD)
        {
            m_EnemyOwner.m_HeadCut = true;
        }
        else if (m_BodyPart == BodyParts3.L_UPPERLEG 
                 || m_BodyPart == BodyParts3.L_LOWERLEG 
                 || m_BodyPart == BodyParts3.L_FOOT)
        {
            m_EnemyOwner.m_LegCut = LegCut.LEFT;
        }
        else if (m_BodyPart == BodyParts3.R_UPPERLEG 
                 || m_BodyPart == BodyParts3.R_LOWERLEG 
                 || m_BodyPart == BodyParts3.R_FOOT)
        {
            m_EnemyOwner.m_LegCut = LegCut.RIGHT;
        }
        else if (m_BodyPart == BodyParts3.L_UPPERARM 
                 || m_BodyPart == BodyParts3.L_FOREARM)
        {
            m_EnemyOwner.m_ArmCut = ArmCut.LEFT;
        }
        else if (m_BodyPart == BodyParts3.R_UPPERARM 
                 || m_BodyPart == BodyParts3.R_FOREARM)
        {
            m_EnemyOwner.m_ArmCut = ArmCut.RIGHT;
        }
        
        m_PlaneCut.Cut();
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