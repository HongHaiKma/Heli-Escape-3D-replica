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
        if (m_CanCollide)
        {
            if (collision.gameObject.GetComponent<IDamageable>() != null)
            {
                collision.gameObject.GetComponent<IDamageable>().OnHit(collision.gameObject.transform.position);
            }
        }
    }
}
