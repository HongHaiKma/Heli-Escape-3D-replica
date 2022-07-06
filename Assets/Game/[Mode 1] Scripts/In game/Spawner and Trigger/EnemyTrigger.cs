using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Dreamteck.Splines;

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

        m_Anim.SetTrigger("OpenDoor");

        if (m_LookCam)
        {
            Vector3 oldPos = LevelController.Instance.tf_CamLookPoint.localPosition;
            CamController.Instance.m_CMCam.Follow = null;
            // CamController.Instance.m_CMCam.Follow = LevelController.Instance.tf_PivotFollower;
            // float yAxis =
            // LevelController.Instance.tf_CamLookPoint.DOMove(tf_LookPoint.position, 1f).OnStart(() =>
            LevelController.Instance.tf_CamLookPoint.DOMove(tf_LookPoint.position, 1f).OnStart(() =>
                {
                    // CamController.Instance.tf
                    // Time.timeScale = 0.5f;
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

                    // LevelController.Instance.tf_CamLookPoint.DOKill();
                    // Vector3 oldPos = LevelController.Instance.tf_CamLookPoint.localPosition;
                    // DOTween.KillAll(true);
                    LevelController.Instance.tf_CamLookPoint.DOLocalMove(oldPos, 1f, true).SetEase(Ease.Linear)
                    // DOVirtual.DelayedCall(1f, null)
                    .OnStart(() =>
                    {
                        // CamController.Instance.m_CMCam.m_LookAt = null;
                        LevelController.Instance.tf_PivotFollower.GetComponent<SplineFollower>().follow = false;
                        // CamController.Instance.m_CMCam.Follow = LevelController.Instance.tf_CamLookPoint;
                        // CamController.Instance.tf_Owner.rotation = Quaternion.LookRotation(LevelController.Instance.tf_CamLookPoint.position - CamController.Instance.tf_Owner.localPosition);
                    }).OnUpdate(() =>
                    {
                        // CamController.Instance.tf_Owner.rotation = Quaternion.LookRotation(LevelController.Instance.tf_CamLookPoint.position - CamController.Instance.tf_Owner.position);
                    }).SetDelay(2.5f).OnComplete(
                        () =>
                        {
                            LevelController.Instance.tf_PivotFollower.GetComponent<SplineFollower>().follow = true;
                            CamController.Instance.m_CMCam.m_LookAt = LevelController.Instance.tf_CamLookPoint;
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
