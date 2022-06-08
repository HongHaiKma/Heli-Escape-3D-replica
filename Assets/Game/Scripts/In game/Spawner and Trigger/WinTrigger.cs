using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WinTrigger : MonoBehaviour, ITriggerble
{
    public Collider col_Owner;

    public void OnTrigger()
    {
        Trigger();
    }
    
    public async UniTask Trigger()
    {
        Time.timeScale = 1f;
        col_Owner.enabled = false;
        CamController.Instance.CamMoveFinish(0.3f);
        EventManager.CallEvent(GameEvent.LEVEL_WIN);
        GameManager.Instance.m_GameLoop = GameLoop.EndGame;
        
        EventManager.CallEvent(GameEvent.DespawnAllPool);
        SimplePool.Release();
        
        await UniTask.Delay(2000);
        PopupCaller.OpenPopup(UIID.POPUP_WIN);
    }
}
