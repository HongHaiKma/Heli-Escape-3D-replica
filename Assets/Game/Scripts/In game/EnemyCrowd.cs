using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RootMotion;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyCrowd : MonoBehaviour
{
    public bool m_Activated;

    public List<Enemy> m_Enemies;

    private void Start()
    {
        if (m_Activated)
        {
            for (int i = 0; i < m_Enemies.Count; i++)
            {
                m_Enemies[i].m_StateMachine.ChangeState(ChaseState.Instance);
            }

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Hostage"))
        {
            for (int i = 0; i < m_Enemies.Count; i++)
            {
                m_Enemies[i].m_StateMachine.ChangeState(ChaseState.Instance);
            }
            gameObject.SetActive(false);
        }
    }

    [Button]
    public void AddEnemyToList()
    {
        GameObject go = transform.parent.gameObject;
        m_Enemies.Clear();
        Enemy[] eee = go.GetComponentsInChildren<Enemy>();
        for (int i = 0; i < eee.Length; i++)
        {
            m_Enemies.Add(eee[i]);
        }
    }
}
