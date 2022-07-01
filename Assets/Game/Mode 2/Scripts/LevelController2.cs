using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RootMotion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController2 : Singleton<LevelController2>
{
    public Transform tf_PivotFollower;
    public List<Transform> tf_Pivots;
    public int m_CurFloor;
    public float m_RotateSpeed;
    public List<Floor> m_Floors;
    public Shooter2 m_Shooter;
    private float TIMESCALE;

    private bool isSlow = false;

    public bool physicSimulate = true;
    
    public Transform tf_MainCamera;


    public Animator anim_IntroCam;

    private async UniTask OnEnable()
    {
        // Physics.autoSimulation = false;
        GameManager.Instance.m_GameMode = GameMode.MODE_2;
        GameManager.Instance.m_GameLoop = GameLoop.Wait;
        
        isSlow = false;
        Time.timeScale = 1;
        m_CurFloor = 0;
        
        GUIManager.Instance.g_Loading.SetActive(false);

        await UniTask.WaitUntil(() => GameManager.Instance.m_GameLoop == GameLoop.Play);
        
        anim_IntroCam.SetTrigger("Intro");

        await UniTask.Delay(2000);
        
        CamController2.Instance.tf_HeliHolder.SetParent(tf_MainCamera);

        CamController2.Instance.ActivateFloor(m_Floors[m_CurFloor]);
        tf_PivotFollower.DOKill();
        tf_PivotFollower.DORotate(new Vector3(0f, -360f, 0f), m_RotateSpeed, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        
        
        tf_PivotFollower.position = tf_Pivots[m_CurFloor].position;
        
        // tf_PivotFollower.DOMove(tf_Pivots[m_CurFloor].position, 2f).OnStart(() =>
        // {
        //     Time.timeScale = 1f;
        //     CamController2.Instance.ActivateFloor(m_Floors[m_CurFloor]);
        //     tf_PivotFollower.DOKill();
        //     tf_PivotFollower.DORotate(new Vector3(0f, -360f, 0f), m_RotateSpeed, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        // });

        EventManager1<bool>.AddListener(GameEvent.SLOWMOTION, OnPivotFollowerSpeed);
    }

    private void OnDisable()
    {
        EventManager1<bool>.RemoveListener(GameEvent.SLOWMOTION, OnPivotFollowerSpeed);
    }

    public override void OnDestroy()
    {
        EventManager1<bool>.RemoveListener(GameEvent.SLOWMOTION, OnPivotFollowerSpeed);
    }

    public void OnPivotFollowerSpeed(bool _slowmotion)
    {
        if (_slowmotion)
        {
            if (!isSlow)
            {
                m_RotateSpeed = 40f;
                tf_PivotFollower.DOKill();
                tf_PivotFollower.DORotate(new Vector3(0f, -360f, 0f), m_RotateSpeed, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                isSlow = true;
            }
            
            
            // Helper.DebugLog("AAAAAAAAAAAAA");
        }
        else
        {
            if (isSlow)
            {
                m_RotateSpeed = 10f;
                tf_PivotFollower.DOKill();
                tf_PivotFollower.DORotate(new Vector3(0f, -360f, 0f), m_RotateSpeed, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                isSlow = false;
            }
        }
    }

    public async UniTask RemoveEnemy(Enemy2 _enemy)
    {
        m_Floors[m_CurFloor].RemoveEnemy(_enemy);
        if (m_Floors[m_CurFloor].m_Enemies.Count <= 0)
        {
            m_CurFloor++;
            if (m_CurFloor > m_Floors.Count - 1)
            {
                await UniTask.Delay(1500);
                // UIIngame2.Instance.go_PopupWin.SetActive(true);
                PopupCaller.OpenPopup(UIID.POPUP_WIN);
                return;
            }
            
            tf_PivotFollower.DOMove(tf_Pivots[m_CurFloor].position, 2f).OnStart(() => Time.timeScale = 1f).OnStart(() =>
            {
                CamController2.Instance.ActivateFloor(m_Floors[m_CurFloor]);
            });
        }
    }

    private void Update()
    {
        // tf_PivotFollower.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        // if (Helper.GetKeyDown(KeyCode.A))
        // {
        //     m_CurFloor++;
        //     tf_PivotFollower.DOLocalMove(tf_Pivots[m_CurFloor].position, 2f);
        // }
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     // Time.timeScale = 0f;
        //     // Physics.autoSimulation = false;
        //     // Physics.Simulate(0.25f * 0.02f);
        //     
        //     physicSimulate = false;
        //     EventManager1<bool>.CallEvent(GameEvent.SLOWMOTION, physicSimulate);
        // }
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     // Time.timeScale = 1f;
        //     // Physics.autoSimulation = true;
        //     
        //     // physicSimulate = true;
        //     // EventManager1<bool>.CallEvent(GameEvent.SLOWMOTION, physicSimulate);
        //     
        //     Helper.DebugLog("Simulation: " + Physics.autoSimulation);
        // }
        //
        // if (physicSimulate)
        // {
        //     // Physics.autoSimulation = true;
        //     Physics.Simulate(Time.fixedDeltaTime);
        // }
        // else
        // {
        //     // Physics.autoSimulation = false;
        //     // Physics.Simulate(Time.fixedDeltaTime * 0.25f);
        //     Physics.Simulate(Time.fixedDeltaTime * 0.25f);
        // }
    }
}
