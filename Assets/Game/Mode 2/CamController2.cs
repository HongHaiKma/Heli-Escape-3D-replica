using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamController2 : Singleton<CamController2>
{
    public List<Enemy2> m_Enemies;
    public bool slow = false;

    private void OnEnable()
    {
        ResetLevel();
    }

    public void ResetLevel()
    {
        Time.timeScale = 1;
        slow = false;
        m_Enemies.Clear();
    }

    public void ActivateFloor(Floor _floor)
    {
        ResetLevel();
        m_Enemies = _floor.m_Enemies;
    }

    private void Update()
    {
        // if (m_Enemies.Count > 0)
        // {
        //     if (Time.timeScale == 1)
        //     {
        //         // if (IsSlow())
        //         // {
        //         //     Time.timeScale = 0.4f;
        //         // }
        //         Time.timeScale = m_Enemies.Any(IsSlow2) ? Time.timeScale = 0.4f : Time.timeScale = 1f;
        //         // m_Enemies.Any(IsSlow2);
        //     }
        //     else if(Time.timeScale == 0.4)
        //     {
        //         // if (!IsSlow())
        //         // {
        //         //     Time.timeScale = 1f;
        //         // }
        //     }
        // }

        if (m_Enemies.Count > 0)
        {
            Time.timeScale = m_Enemies.Any(IsSlow2) ? Time.timeScale = 0.4f : Time.timeScale = 1f;
        }
    }

    public bool IsSlow()
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            Vector3 targetDir = transform.position - m_Enemies[i].tf_Owner.position;
            targetDir = targetDir.normalized;
       
            float dot = Vector3.Dot(targetDir, m_Enemies[i].tf_Owner.forward);
            float angle = Mathf.Acos( dot ) * Mathf.Rad2Deg;

            if (angle < 60f)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsSlow2(Enemy2 _enemy)
    {
        Vector3 targetDir = transform.position - _enemy.tf_Owner.position;
        targetDir = targetDir.normalized;
       
        float dot = Vector3.Dot(targetDir, _enemy.tf_Owner.forward);
        float angle = Mathf.Acos( dot ) * Mathf.Rad2Deg;

        if (angle < 60f)
        {
            return true;
        }

        return false;
    }
}
