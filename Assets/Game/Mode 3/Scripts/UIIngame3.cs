using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIngame3 : Singleton<UIIngame3>
{
    public Transform tf_MainCanvas;
    public GameObject go_TapToPlay;
    public GameObject go_Shop;
    public GameObject go_TrackPad;

    private void OnEnable()
    {
        go_TapToPlay.SetActive(true);
        go_Shop.SetActive(true);
        go_TrackPad.SetActive(false);
    }

    public void PlayGame()
    {
        GameManager.Instance.m_GameLoop = GameLoop.Play;
        go_TapToPlay.SetActive(false);
        go_Shop.SetActive(false);
        go_TrackPad.SetActive(true);
    }

    public void OpenInventory()
    {
        PopupCaller.OpenPopup(UIID.POPUP_INVENTORY);
    }

    public void Test()
    {
        Helper.DebugLog("AAAAAAAAAAAA");
    }
}
