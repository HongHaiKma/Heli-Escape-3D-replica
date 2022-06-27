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
    

    private void Update()
    {
        timeScale = Time.timeScale;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Helper.DebugLog("Time Scale: " + Time.timeScale);
        }
        
        // GraphNode nearestNode = AstarPath.active.GetNearest(tf_Owner.position, NNConstraint.Default).node;
        // if (nearestNode != null)
        // {
        //   if (nearestNode.Walkable)
        //           {
        //               Int3 a = new Int3();
        //               Vector3 b = (Vector3)a;
        //                              
        //               // m_AI.destination = b;
        //               rb_Owner.position = (Vector3)nearestNode.position;
        //           }  
        // }
    }

    public void LoadSceneTest()
    {
        // SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Single);
        GUIManager.Instance.LoadPlayScene();
    }

    // public async UniTask LoadLevelTask()
    // {
    //     if (LevelController.Instance != null)
    //     {
    //         UIIngame.Instance.img_Flash.gameObject.SetActive(true);
    //         UIIngame.Instance.img_Flash.DOFade(1, 0.3f);
    //         await UniTask.Delay(System.TimeSpan.FromSeconds(0.4f));
    //         // CamController.Instance.ResetLevel();
    //         Destroy(LevelController.Instance.gameObject);
    //         SimplePool.Release();
    //         Resources.UnloadUnusedAssets();
    //         System.GC.Collect();
    //
    //         await UniTask.WaitUntil(() => m_LevelLoaded == false);
    //     }
    //     // else
    //     // {
    //     //     await UniTask.WhenAll(CamController.Instance.CameraIntro(new Vector3(0f, 5f, 0f), 1.5f)); 
    //     // }
    //
    //     int level = ProfileManager.GetLevel();
    //     AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>("Level_" + level.ToString());
    //
    //     await goHandle;
    //     
    //     if (goHandle.Status == AsyncOperationStatus.Succeeded)
    //     {
    //         GameObject obj = goHandle.Result;
    //         Instantiate(obj).GetComponent<Transform>().parent = UIIngame.Instance.tf_LevelHolder;
    //     }
    //
    //     await UniTask.WaitForEndOfFrame();
    //     
    //     // if (LevelController.Instance == null)
    //     // {
    //         
    //
    //     // CamController.Instance.CameraIntro(new Vector3(0f, 5f, 0f), 1.5f);
    //
    //     CamController.Instance.m_CMCam.Follow = LevelController.Instance.tf_CamLookPoint;
    //     CamController.Instance.m_CMCam.LookAt = LevelController.Instance.tf_CamLookPoint;
    // }
}

public enum GameLoop
{
    Wait,
    Play,
    WaitEndGame,
    GameWin,
    GameLose,
}