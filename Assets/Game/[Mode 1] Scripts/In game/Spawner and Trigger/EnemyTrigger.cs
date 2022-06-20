using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour, ITriggerble
{
    public Collider col_Owner;
    public EnemySpawner m_EnemySpawner;

    public void OnTrigger()
    {
        col_Owner.enabled = false;
        
        if (m_EnemySpawner.isSpawned)
        {
            if (m_EnemySpawner.isWaiting)
            {
                m_EnemySpawner.EnemyRun();
            }
        }
        else
        {
            m_EnemySpawner.SpawnEnemy();
        }
    }
}
