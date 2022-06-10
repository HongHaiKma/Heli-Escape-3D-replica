using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton2<InGameManager>
{
    public Transform tf_LevelHolder;
    public Image img_Flash;
    public Image img_Crosshair;
    public Transform tf_MainCanvas;
    
    // [Header("Combo")]
    // public Animator m_AnimUI;
    // public float m_ComboTime;
    // public int m_Combo;
    // public TextMeshProUGUI txt_Combo;
    // public GameObject go_Combo;

    public override void OnEnable()
    {
        // m_Combo = 0;
        // go_Combo.SetActive(false);
    }

    private void Update()
    {
        // if (m_ComboTime > 0)
        // {
        //     m_ComboTime -= Time.deltaTime;
        // }
    }

    public void ResetLevel()
    {
        // m_Combo = 0;
        // go_Combo.SetActive(false);
    }

    public async UniTask Combo()
    {
        // if (m_ComboTime > 0) m_Combo++;
        // else m_Combo = 1;
        //
        // m_ComboTime = 2f;
        //
        // txt_Combo.text = "X" + m_Combo;
        // go_Combo.SetActive(false);
        // await UniTask.Delay(10);
        // m_AnimUI.SetTrigger("Combo");
    }
    
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         PopupCaller.OpenPopup(UIID.POPUP_WIN);
    //     }
    //     
    //     if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         PopupCaller.OpenPopup(UIID.POPUP_LOSE);
    //     }
    // }
}
