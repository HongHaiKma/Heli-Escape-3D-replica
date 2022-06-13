using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager2 : Singleton<InGameManager2>
{
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
