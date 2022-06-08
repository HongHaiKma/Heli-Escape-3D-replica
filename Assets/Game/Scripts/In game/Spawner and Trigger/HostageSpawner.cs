using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SRF;
using Unity.VisualScripting;
using UnityEngine;

public class HostageSpawner : MonoBehaviour, ITriggerble
{
    public bool isWaiting;
    public List<Transform> tf_RandomPoints;
    public int m_HostageNumber;
    public HostageSpawn m_HostageSpawn;
    public BoxCollider m_Collider;
    public List<Hostage> m_Hostages;

    private async UniTask OnEnable()
    {
        m_Collider.enabled = isWaiting;
        
        List<Hostage> a = new List<Hostage>();

        for (int i = 0; i < m_HostageNumber; i++)
        {
            Transform randomPos = tf_RandomPoints[i % tf_RandomPoints.Count];
            if (!isWaiting)
            {
                
                Hostage hos = PrefabManager.Instance.SpawnHostagePool(m_HostageSpawn.ToString(), randomPos.position)
                    .GetComponent<Hostage>();
                hos.transform.parent = LevelController.Instance.transform;
                m_Hostages.Add(hos);
                LevelController.Instance.m_HostageRun.Add(hos);
                // await UniTask.WaitUntil(() => hos.isAwake == true);
                await UniTask.WaitUntil(() => hos.isActiveAndEnabled == true);
                await UniTask.WaitForEndOfFrame();
                hos.ChangeState(P_RunState.Instance);
            }
            else
            {
                Hostage hos = PrefabManager.Instance
                    .SpawnHostagePool(m_HostageSpawn.ToString(), randomPos.position)
                    .GetComponent<Hostage>();
                hos.transform.parent = LevelController.Instance.transform;
                m_Hostages.Add(hos);
                LevelController.Instance.m_HostageWait.Add(hos);
            }

            // a.Add(PrefabManager.Instance.SpawnHostagePool(m_HostageSpawn.ToString(), randomPos.position)
            //     .GetComponent<Hostage>());
        }

        // for (int i = 0; i < m_HostageNumber; i++)
        // {
        //     if (!isWaiting) a[i].ChangeState(P_RunState.Instance);
        // }

        // if (!isWaiting)
        // {
        //     for (int i = 0; i < m_Hostages.Count; i++)
        //     {
        //         m_Hostages[i].ChangeState(P_RunState.Instance);
        //     }
        // }
    }

    public void OnTrigger()
    {
        m_Collider.enabled = false;
        for (int i = 0; i < m_Hostages.Count; i++)
        {
            LevelController.Instance.m_HostageRun.Add(m_Hostages[i]);
            m_Hostages[i].ChangeState(P_RunState.Instance);
        }
    }
}

public enum HostageSpawn
{
    Hostage_01,
}

interface ITriggerble
{
    void OnTrigger();
}