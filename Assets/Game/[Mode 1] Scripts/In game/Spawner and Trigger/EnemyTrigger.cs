using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour, ITriggerble
{
    public Collider col_Owner;
    public EnemySpawner m_EnemySpawner;
    public Transform tf_LookPoint;

    public bool m_LookCam = false;

    public Animator m_Anim;

    public void OnTrigger()
    {
        col_Owner.enabled = false;

        if (m_LookCam)
        {
            Vector3 oldPos = LevelController.Instance.tf_CamLookPoint.localPosition;
            CamController.Instance.m_CMCam.Follow = null;
            // CamController.Instance.m_CMCam.Follow = LevelController.Instance.tf_PivotFollower;
            LevelController.Instance.tf_CamLookPoint.DOMove(tf_LookPoint.position, 1f).OnStart(() =>
                {
                    Time.timeScale = 0.5f;
                    m_Anim.SetTrigger("OpenDoor");
                    Helper.DebugLog("LLLLLLLLLLLLLLLLLL");
                })
                .OnComplete(
                () =>
                {
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
                    
                    LevelController.Instance.tf_CamLookPoint.DOLocalMove(oldPos, 1f, true).SetDelay(1.5f).OnComplete(
                        () =>
                        {
                            CamController.Instance.m_CMCam.Follow = LevelController.Instance.tf_CamLookPoint;
                            Time.timeScale = 1f;
                        });
                });
        }
        else
        {
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

    [Button]
    public void TestAnim()
    {
        m_Anim.SetTrigger("OpenDoor");
    }
}
