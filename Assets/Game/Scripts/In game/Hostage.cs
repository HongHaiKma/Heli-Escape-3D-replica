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
    public Rigidbody rb_Owner;
    public Transform tf_Onwer;
    public Animator m_Anim;

    public Transform tf_LookAtPoint;

    public AIPath m_AI;

    [Header("States")]
    public StateMachine<Hostage> m_StateMachine;

    public HostageStates m_HostageStates;

    private void OnEnable()
    {
        m_AI.isStopped = false;
        rb_Owner.useGravity = true;
        m_StateMachine = new StateMachine<Hostage>(this);
        m_StateMachine.Init(P_WaitState.Instance);
        
        EventManager.AddListener(GameEvent.LEVEL_WIN, OnHostageWin);
        // EventManager.AddListener(GameEvent.DespawnAllPool, DestroyAllPool);
    }

    private void OnDisable()
    {
        m_StateMachine.ChangeState(P_WaitState.Instance);
        EventManager.RemoveListener(GameEvent.LEVEL_WIN, OnHostageWin);
        // EventManager.RemoveListener(GameEvent.DespawnAllPool, DestroyAllPool);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(GameEvent.LEVEL_WIN, OnHostageWin);
        // EventManager.RemoveListener(GameEvent.DespawnAllPool, DestroyAllPool);
    }

    private void Update()
    {
        m_StateMachine.ExecuteStateUpdate();
    }
    
    public void DestroyAllPool()
    {
        if(gameObject.activeInHierarchy == true)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
        }
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

    public void OnHostageWin()
    {
        // if (GameManager.Instance.m_GameLoop != GameLoop.EndGame)
        // {
            WinTask(); 
        // }
    }

    public async UniTask WinTask()
    {
        // ProfileManager.PassLevel();
        // m_Anim.SetTrigger("JumpHeli");
        // EventManager.CallEvent(GameEvent.LEVEL_WIN);
        // await UniTask.Delay(500, true);
        // Time.timeScale = 0.5f;
        // await UniTask.WhenAll(CamController.Instance.CameraOutro());
        tf_Onwer.position = LevelController.Instance.tf_JumpHeliPos.position;
        ChangeState(P_JumpToHeli.Instance);
        await UniTask.Delay(200);
        tf_Onwer.DOMove(CamController.Instance.tf_HeliHolderPoint.position, 1.5f).OnComplete(() =>
        {
            tf_Onwer.parent = CamController.Instance.tf_HeliHolderPoint;
        });
        // await UniTask.Delay(2000);
        // PopupCaller.OpenPopup(UIID.POPUP_WIN);
    }

    #region States
    
    public void ChangeState(IState<Hostage> state)
    {
        m_StateMachine.ChangeState(state);
    }

    public void OnRunEnter()
    {
        m_HostageStates = HostageStates.RUN;
        m_AI.canMove = true;
        m_AI.isStopped = false;
        m_Anim.SetTrigger("Run");
    }
    
    public void OnRunExecute()
    {
        // if (m_Anim.tri)
        // {
        //     
        // }
        m_AI.destination = LevelController.Instance.tf_PivotFollower.position;
    }
    
    public void OnRunExit()
    {
        
    }
    
    public void OnJumpToHeliEnter()
    {
        m_HostageStates = HostageStates.JUMP_HELI;
        m_AI.canMove = false;
        m_AI.isStopped = true;
        rb_Owner.useGravity = false;
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
        LevelController.Instance.m_HostageRun.Remove(this);
        
        await UniTask.Delay(1000);
        
        if(gameObject.activeInHierarchy == true)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
        }

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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("DeadZone"))
        {
            ChangeState(P_DeathState.Instance);
            Helper.DebugLog("HOSTAGE DIEEEEEEEEEEEE");
        }
    }

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