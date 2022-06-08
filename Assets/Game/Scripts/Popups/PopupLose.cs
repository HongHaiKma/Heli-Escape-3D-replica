using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLose : UICanvas
{
    public Button btn_ReplayLevel;
    
    private void Awake()
    {
        m_ID = UIID.POPUP_LOSE;
        Init();

        GUIManager.Instance.AddClickEvent(btn_ReplayLevel, OnReplayLevel);
    }

    public void OnReplayLevel()
    {
        // EventManager.CallEvent(GameEvent.DespawnAllPool);
        // // CamController.Instance.ResetLevel();
        // InGameManager.Instance.ResetLevel();
        // GameManager.Instance.ResetLevel();
        // GameManager.Instance.LoadLevelTask();
        OnClose();
        
        GameManager.Instance.m_GameLoop = GameLoop.Wait;
        
        GameManager.Instance.LoadSceneTest();
    }
}
