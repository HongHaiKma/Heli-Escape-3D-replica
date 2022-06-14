using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : UICanvas
{
    public Button btn_NextLevel;

    private void Awake()
    {
        m_ID = UIID.POPUP_WIN;
        Init();

        GUIManager.Instance.AddClickEvent(btn_NextLevel, OnNextLevel);
    }

    public void OnNextLevel()
    {
        // EventManager.CallEvent(GameEvent.DespawnAllPool);
        // InGameManager.Instance.ResetLevel();
        // GameManager.Instance.ResetLevel();
        // // GameManager.Instance.LoadLevel();
        // GameManager.Instance.LoadLevelTask();
        ProfileManager.SetLevel(ProfileManager.GetLevel() + 1);
        
        OnClose();

        GameManager.Instance.m_GameLoop = GameLoop.Wait;

        // GameManager.Instance.LoadSceneTest();
        // StartCoroutine(GUIManager.Instance.LoadPlayScene());
        GUIManager.Instance.LoadPlayScene();
    }
}
