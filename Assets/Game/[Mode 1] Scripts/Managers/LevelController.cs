using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("GameWin")] 
    public Transform tf_CamPos;
    public Transform tf_CamLook;
    public Transform tf_CamFinish;
    public Transform tf_JumpHeliPos;

    public async UniTask OnEnable()
    {
        m_HostageRun.Clear();
        m_HostageWait.Clear();
        
        if (UIIngame.Instance.img_Flash.gameObject.activeInHierarchy) 
            Trasition();
        else 
            CamController.Instance.CameraIntro(new Vector3(15f, 15f, -4.5f), 1.5f);

        await UniTask.WaitUntil(() => GUIManager.Instance != null);
        GUIManager.Instance.g_Loading.SetActive(false);
        GameManager.Instance.m_LevelLoaded = true;
    }

    async UniTask Trasition()
    {
        CamController.Instance.CameraIntro(new Vector3(15f, 15f, -4.5f), 1.5f);
        await UniTask.Delay(100);
        UIIngame.Instance.img_Flash.DOFade(0f, 1f);
        await UniTask.Delay(1000);
        UIIngame.Instance.img_Flash.gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        GameManager.Instance.m_LevelLoaded = false;
    }

    public Hostage FindNearestHostage(Vector3 _pos)
    {
        var target = m_HostageRun.OrderBy(x => (x.tf_Onwer.position - _pos).sqrMagnitude).First().GetComponent<Hostage>();
        
        return target;
    }
}
