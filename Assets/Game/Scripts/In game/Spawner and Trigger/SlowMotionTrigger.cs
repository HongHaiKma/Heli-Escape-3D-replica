using System.Collections;
using System.Collections.Generic;
using RootMotion;
using UnityEngine;

public class SlowMotionTrigger : MonoBehaviour, ITriggerble
{
    public Collider m_Collider;
    
    public void OnTrigger()
        {
            m_Collider.enabled = false;
            CamController.Instance.CamMoveEndPos(3f);
        }
}
