using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using SRF;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Hostage : MonoBehaviour
{
    public Transform tf_Onwer;
    public Animator m_Anim;

    public Transform tf_LookAtPoint;

    public AIPath m_AI;

    [Header("States")]
    public StateMachine<Hostage> m_StateMachine;

    public HostageStates m_HostageStates;

    public bool isAwake = false;

    private void Awake()
    {
        m_StateMachine = new StateMachine<Hostage>(this);
        m_StateMachine.Init(P_WaitState.Instance);
        
        // isAwake = true;
    }

    private void OnEnable()
    {
        isAwake = true;
    }

    private void Update()
    {
        m_StateMachine.ExecuteStateUpdate();
    }

    public async UniTask Death()
    {
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
        // await UniTask.WhenAll(CamController.Instance.CameraOutro());
        tf_Onwer.DOMove(CamController.Instance.tf_HeliHolderPoint.position, 1f).OnComplete(() =>
        {
            tf_Onwer.parent = CamController.Instance.tf_HeliHolderPoint;
        });
        await UniTask.Delay(2000);
        PopupCaller.OpenPopup(UIID.POPUP_WIN);
    }

    #region States
    
    public void ChangeState(IState<Hostage> state)
    {
        m_StateMachine.ChangeState(state);
    }

    public void OnRunEnter()
    {
        m_HostageStates = HostageStates.RUN;
        m_Anim.SetTrigger("Run");
    }
    
    public void OnRunExecute()
    {
        m_AI.destination = LevelController.Instance.tf_PivotFollower.position;
    }
    
    public void OnRunExit()
    {
        
    }
    
    public void OnJumpToHeliEnter()
    {
        m_HostageStates = HostageStates.JUMP_HELI;
        m_Anim.SetTrigger("JumpHeli");
    }
    
    public void OnJumpToHeliExecute()
    {
        
    }
    
    public void OnJumpToHeliExit()
    {
        
    }
    
    public async UniTask OnDeathEnter()
    {
        m_HostageStates = HostageStates.DEATH;
        m_Anim.SetTrigger("Death");
        
        await UniTask.Delay(1000);
        
        LevelController.Instance.m_HostageRun.Remove(this);
        PrefabManager.Instance.DespawnPool(gameObject);

        if (LevelController.Instance.m_HostageRun.Count <= 0)
        {
            EventManager.CallEvent(GameEvent.LEVEL_LOSE);
            await UniTask.Delay(1000);
            PopupCaller.OpenPopup(UIID.POPUP_LOSE);
        }
    }
    
    public void OnDeathExecute()
    {

    }

    public void OnDeathExit()
    {
        
    }
    
    public void OnWaitEnter()
    {
        m_HostageStates = HostageStates.WAIT;
        m_Anim.SetTrigger("Wait");
    }
    
    public void OnWaitExecute()
    {
        
    }
    
    public void OnWaitExit()
    {
        
    }

    #endregion

    #region Collision

    private void OnTriggerEnter(Collider other)
    {
        if (m_HostageStates == HostageStates.RUN)
        {
            ITriggerble trigger = other.GetComponent<ITriggerble>();
            if (trigger != null)
                trigger.OnTrigger();  
        }
    }

    #endregion
}

public enum HostageStates
{
    WAIT,
    RUN,
    DEATH,
    JUMP_HELI,
}