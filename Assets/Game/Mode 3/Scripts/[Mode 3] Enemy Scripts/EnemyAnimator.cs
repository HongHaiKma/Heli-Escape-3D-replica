using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Enemy3 m_EnemyOwner;

    public void EnemyAttack()
    {
        ChangeState(AttackState3.Instance);
    }
    
    public void ChangeStateChase()
    {
        Helper.DebugLog("Change Chase State");
        // m_EnemyOwner.ChangeState(ChaseState3.Instance);
        
        if (LevelController3.Instance.m_Hostages.Count <= 0)
        {
            ChangeState(IdleState3.Instance);
        }
        else
        {
            ChangeState(ChaseState3.Instance);
        }
    }

    public void ChangeState(IState<Enemy3> _enemy)
    {
        m_EnemyOwner.ChangeState(_enemy);
    }
}
