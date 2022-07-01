using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIIngame2 : Singleton<UIIngame2>
{
    public Transform tf_MainCanvas;
    public GameObject go_TapToPlay;

    private void OnEnable()
    {
        go_TapToPlay.SetActive(true);
    }

    public void PlayGame()
    {
        GameManager.Instance.m_GameLoop = GameLoop.Play;
        go_TapToPlay.SetActive(false);
    }
}
