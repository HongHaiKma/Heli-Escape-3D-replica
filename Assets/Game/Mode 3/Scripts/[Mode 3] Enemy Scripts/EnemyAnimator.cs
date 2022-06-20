using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Enemy3 m_EnemyOwner;

    public void EnemyAttack()
    {
        Helper.DebugLog("AAAAAAAAAAAAAAA");
    }
    
    public void ChangeStateChase()
    {
        Helper.DebugLog("BBBBBBBBBBBBBBB");
        // m_EnemyOwner.ChangeState(ChaseState3.Instance);
    }
}
