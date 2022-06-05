using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    public Transform tf_CamLookPoint;
    public Transform tf_PivotFollower;

    public List<Hostage> m_HostageRun;
    public List<Hostage> m_HostageWait;

    public void OnEnable()
    {
        m_HostageRun.Clear();
        m_HostageWait.Clear();
        
        if (InGameManager.Instance.img_Flash.gameObject.activeInHierarchy) 
            Trasition();
        else 
            CamController.Instance.CameraIntro(new Vector3(0f, 5f, 0f), 1.5f);

        GUIManager.Instance.g_Loading.SetActive(false);
        GameManager.Instance.m_LevelLoaded = true;
    }

    async UniTask Trasition()
    {
        CamController.Instance.CameraIntro(new Vector3(0f, 5f, 0f), 1.5f);
        await UniTask.Delay(100);
        InGameManager.Instance.img_Flash.DOFade(0f, 1f);
        await UniTask.Delay(1000);
        InGameManager.Instance.img_Flash.gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        GameManager.Instance.m_LevelLoaded = false;
    }
}
