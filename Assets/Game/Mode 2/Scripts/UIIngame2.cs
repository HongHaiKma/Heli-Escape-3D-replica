using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIIngame2 : Singleton<UIIngame2>
{
    public Transform tf_MainCanvas;
    public GameObject go_PopupWin;
    public GameObject go_PopupLose;

    private void OnEnable()
    {
        go_PopupWin.SetActive(false);
        go_PopupLose.SetActive(false);
    }

    public void OnClick()
    {
        SceneManager.LoadScene("PlaySceneMode2");
    }
}
