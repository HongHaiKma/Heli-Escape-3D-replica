using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapItem : MonoBehaviour
{
    public float m_Time = 0f;
    public bool m_CanCollide = true;
    public bool m_Activated = false;

    private void Update()
    {
        if (m_Activated)
        {
            m_Time += Time.deltaTime;
        }

        if (m_Time > 5f)
        {
            m_CanCollide = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Helper.DebugLog("Collide");
        if (m_CanCollide)
        {
            Helper.DebugLog("Can Collide");
            if (collision.gameObject.GetComponent<IDamageable>() != null)
            {
                Helper.DebugLog("Collide Enemy");
                collision.gameObject.GetComponent<IDamageable>().OnHit(collision.gameObject.transform.position);
            }

            if (collision.gameObject.tag.Equals("Enemy") || collision.gameObject.tag.Equals("EnemyHead"))
            {
                Helper.DebugLog("Collide ENEMYEYYYY");
            }
        }
    }
}
