using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIIngame2 : Singleton<UIIngame2>
{
    public Transform tf_MainCanvas;
    public GameObject go_TapToPlay;
    public GameObject go_Shop;

    private void OnEnable()
    {
        go_TapToPlay.SetActive(true);
        go_Shop.SetActive(true);
    }

    public void PlayGame()
    {
        GameManager.Instance.m_GameLoop = GameLoop.Play;
        go_TapToPlay.SetActive(false);
        go_Shop.SetActive(false);
    }

    public void OpenInventory()
    {
        PopupCaller.OpenPopup(UIID.POPUP_INVENTORY);
    }
}
