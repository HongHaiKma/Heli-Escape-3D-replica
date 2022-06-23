using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrapSystem : MonoBehaviour, ITrap
{
    public Collider m_Key;
    public ParticleSystem m_VFXOwner;
    public bool m_Activated = false;
    public List<TrapItem> m_Items;

    public void OnTrigger()
    {
        if (!m_Activated)
        {
            ActivateTrap();
        }
    }

    public virtual void ActivateTrap()
    {
        m_Key.enabled = false;
        m_VFXOwner.Play();
        m_Activated = true;
        for (int i = 0; i < m_Items.Count; i++)
        {
            m_Items[i].m_Activated = true;
        }
    }

    [Button]
    public void TestTrap()
    {
        OnTrigger();
    }
}