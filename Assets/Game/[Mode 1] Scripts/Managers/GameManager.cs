using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dreamteck.Splines.Primitives;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[DefaultExecutionOrder(-92)]
public class GameManager : Singleton2<GameManager>
{
    public GameMode m_GameMode;
    
    public int m_EnemyWarning;

    public bool m_LevelLoaded;

    public GameLoop m_GameLoop;

    public float timeScale;
    // public Transform tf_Shooter;

    private void Start()
    {
        Application.targetFrameRate = 90;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_LevelLoaded = false;
        m_GameLoop = GameLoop.Wait;
        m_EnemyWarning = 0;
    }

    public void ResetLevel()
    {
        m_EnemyWarning = 0;
        m_GameLoop = GameLoop.Wait;
    }
    

    // private void Update()
    // {
    //     // timeScale = Time.timeScale;
    //     // if (Input.GetKeyDown(KeyCode.A))
    //     // {
    //     //     // Time.timeScale = 0f;
    //     //     Physics.autoSimulation = false;
    //     //     Physics.Simulate(0.25f * 0.02f);
    //     // }
    //     // if (Input.GetKeyDown(KeyCode.D))
    //     // {
    //     //     // Time.timeScale = 1f;
    //     //     Physics.autoSimulation = true;
    //     // }
    //
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         Time.timeScale = 0.25f;
    //     }
    //     
    //     // GraphNode nearestNode = AstarPath.active.GetNearest(tf_Owner.position, NNConstraint.Default).node;
    //     // if (nearestNode != null)
    //     // {
    //     //   if (nearestNode.Walkable)
    //     //           {
    //     //               Int3 a = new Int3();
    //     //               Vector3 b = (Vector3)a;
    //     //                              
    //     //               // m_AI.destination = b;
    //     //               rb_Owner.position = (Vector3)nearestNode.position;
    //     //           }  
    //     // }
    // }

    public void LoadSceneTest()
    {
        // SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Single);
        GUIManager.Instance.LoadPlayScene();
    }
}

public enum GameLoop
{
    Wait,
    Play,
    WaitEndGame,
    GameWin,
    GameLose,
}

public enum GameMode
{
    MODE_1 = 1,
    MODE_2 = 2,
    MODE_3 = 3,
}