using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemy3Bar : WarningUI
{
    public Enemy3 m_EnemyOwner;
    public Image img_Health;

    private void OnEnable()
    {
        img_Health.fillAmount = (m_EnemyOwner.m_Health / 4f);
    }

    public void UpdateHealth()
    {
        img_Health.fillAmount = (m_EnemyOwner.m_Health / 4f);
    }
}
