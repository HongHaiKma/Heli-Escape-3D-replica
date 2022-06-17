using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController3 : Singleton<LevelController3>
{
    public float m_TimeScale;
    
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
