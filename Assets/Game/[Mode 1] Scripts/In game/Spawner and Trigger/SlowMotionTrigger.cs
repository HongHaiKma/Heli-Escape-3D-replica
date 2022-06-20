using System.Collections;
using System.Collections.Generic;
using RootMotion;
using UnityEngine;

public class SlowMotionTrigger : MonoBehaviour, ITriggerble
{
    public Collider m_Collider;
    
    public void OnTrigger()
    {
        Time.timeScale = 0.5f;
        GameManager.Instance.m_GameLoop = GameLoop.WaitEndGame;
        m_Collider.enabled = false;
        CamController.Instance.CamMoveEndPos(3f);
    }
}
