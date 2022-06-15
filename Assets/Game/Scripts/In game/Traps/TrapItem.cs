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

        if (m_Time > 7f)
        {
            m_CanCollide = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_CanCollide)
        {
            IDamageable iDmg = collision.gameObject.GetComponent<IDamageable>();
            if (iDmg != null)
            {
                iDmg.OnHit(collision.gameObject.transform.position);
            }
        }
    }
}
