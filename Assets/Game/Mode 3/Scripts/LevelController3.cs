using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController3 : Singleton<LevelController3>
{
    public List<Hostage3> m_Hostages;
    public GameObject go_TrackPad;

    public float m_TimeScale;

    private void OnEnable()
    {
        Time.timeScale = 1;
        GUIManager.Instance.g_Loading.SetActive(false);
    }

    private void Update()
    {
        if (Helper.GetKeyDown(KeyCode.A))
        {
            m_TimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        if (Helper.GetKeyDown(KeyCode.D))
        {
            Time.timeScale = m_TimeScale;
        }
    }
}
