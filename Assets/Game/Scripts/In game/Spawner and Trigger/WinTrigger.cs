using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour, ITriggerble
{
    public Collider col_Owner;
    public void OnTrigger()
    {
        Time.timeScale = 1f;
        col_Owner.enabled = false;
        CamController.Instance.CamMoveFinish(0.3f);
        EventManager.CallEvent(GameEvent.LEVEL_WIN);
    }
}
