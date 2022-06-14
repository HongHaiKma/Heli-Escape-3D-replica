using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SRF;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool isSpawned;
    public bool isWaiting;
    public bool isGoup;
    public List<Transform> tf_RandomPoints;
    public int m_Number;
    public Enemyname m_EnemyName;
    public List<Enemy> m_Enemies;
    public EnemyTrigger m_EnemyTrigger;

    private async UniTask OnEnable()
    {
        if (isSpawned)
        {
            if (isWaiting)
            {
                m_EnemyTrigger.col_Owner.enabled = true;

                for (int i = 0; i < m_Number; i++)
                {
                    Transform randomPos = tf_RandomPoints[i % tf_RandomPoints.Count];
                    Enemy hos = PrefabManager.Instance.SpawnEnemyPool(m_EnemyName.ToString(), randomPos.position)
                        .GetComponent<Enemy>();
                    hos.transform.parent = LevelController.Instance.transform;
                    m_Enemies.Add(hos);
                }
            }
            else
            {
                m_EnemyTrigger.col_Owner.enabled = false;

                for (int i = 0; i < m_Number; i++)
                {
                    Transform randomPos = tf_RandomPoints[i % tf_RandomPoints.Count];
                    Enemy hos = PrefabManager.Instance.SpawnEnemyPool(m_EnemyName.ToString(), randomPos.position)
                        .GetComponent<Enemy>();
                    hos.transform.parent = LevelController.Instance.transform;
                    m_Enemies.Add(hos);
                    await UniTask.WaitUntil(() => hos != null && hos.isActiveAndEnabled == true);
                    // await UniTask.WaitUntil(() => hos.isActiveAndEnabled == true);
                    await UniTask.WaitForEndOfFrame();
                    // if (isGoup)
                    // {
                    //     hos.ChangeState(ClimbState.Instance);
                    // }
                    // else
                    // {
                    //     hos.ChangeState(ChaseState.Instance);  
                    // }
                    if (!isGoup)
                    {
                        hos.ChangeState(ChaseState.Instance);
                    }
                }
            }
        }
        else
        {
            isWaiting = false;
            m_EnemyTrigger.col_Owner.enabled = true;
        }
    }

    public async UniTask SpawnEnemy()
    {
        for (int i = 0; i < m_Number; i++)
        {
            Transform randomPos = tf_RandomPoints[i % tf_RandomPoints.Count];
            Enemy hos = PrefabManager.Instance.SpawnEnemyPool(m_EnemyName.ToString(), randomPos.position)
                .GetComponent<Enemy>();
            hos.transform.parent = LevelController.Instance.transform;
            m_Enemies.Add(hos);
            await UniTask.WaitUntil(() => hos.isActiveAndEnabled == true);
            await UniTask.WaitForEndOfFrame();
            // if (isGoup)
            // {
            //     hos.ChangeState(ClimbState.Instance);
            // }
            // else
            // {
            //     hos.ChangeState(ChaseState.Instance);  
            // }
            if (!isGoup)
            {
                hos.ChangeState(ChaseState.Instance);
            }
        }
    }

    public void EnemyRun()
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            m_Enemies[i].m_StateMachine.ChangeState(ChaseState.Instance);
        } 
    }
}

public enum Enemyname
{
    Enemy_1,
}