using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SRF;
using Unity.VisualScripting;
using UnityEngine;

public class Hostage : MonoBehaviour, IDamageable
{
    public Transform tf_Onwer;
    public Animator m_Anim;
    public DOTweenPath m_DotPath;
    public Transform tf_LookAtPoint;

    public bool death;
    

    private void OnEnable()
    {
        death = false;
        m_Anim.SetTrigger("Run");
        
        tf_LookAtPoint.DOKill();
        Vector3 origin = new Vector3(0f, (tf_LookAtPoint.position.y - 60f) + 100f, 0f);
        Vector3 end = new Vector3(0f, (tf_LookAtPoint.position.y + 60f) + 100f, 0f);
        // DOLocalRotateQuaternion(Quaternion.Euler(0f, m_YAxis, 0f), 1.5f)
        LevelController.Instance.m_Hostage.tf_LookAtPoint.DOLocalRotate(end, 5f, RotateMode.Fast).SetLoops(-1, LoopType.Yoyo).startValue = origin;
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         // m_DotPath.
    //     }
    // }

    public async UniTask Death()
    {
        death = true;
        m_Anim.SetTrigger("Death");
        gameObject.RemoveComponentIfExists<DOTweenPath>();
        EventManager.CallEvent(GameEvent.LEVEL_LOSE);
        await UniTask.WhenAll(CamController.Instance.CameraDeath(new Vector3(2.9f, -0.2f, 3.47f), 1f));
        await UniTask.Delay(2000);
        PopupCaller.OpenPopup(UIID.POPUP_LOSE);
    }

    public void Win()
    {
        WinTask();
    }

    public async UniTask WinTask()
    {
        ProfileManager.PassLevel();
        m_Anim.SetTrigger("JumpHeli");
        EventManager.CallEvent(GameEvent.LEVEL_WIN);
        await UniTask.Delay(500, true);
        Time.timeScale = 0.5f;
        await UniTask.WhenAll(CamController.Instance.CameraOutro());
        tf_Onwer.DOMove(CamController.Instance.tf_HeliHolderPoint.position, 1f).OnComplete(() =>
        {
            tf_Onwer.parent = CamController.Instance.tf_HeliHolderPoint;
        });
        await UniTask.Delay(2000);
        PopupCaller.OpenPopup(UIID.POPUP_WIN);
    }

    public void OnHit(Vector3 _pos)
    {
        Death();
        
        PrefabManager.Instance.SpawnVFXPool("VFX_4", _pos);
    }
}
