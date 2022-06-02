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
    // public Transform tf_Shooter;

    private void Start()
    {
        Application.targetFrameRate = 90;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        m_EnemyWarning = 0;
        m_LevelLoaded = false;
    }

    public void ResetLevel()
    {
        m_EnemyWarning = 0;
    }
    

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         Debug.Log("Level: " + ProfileManager.GetLevel());
    //     }
    // }

    public void SetSlowmotion(bool _warning)
    {
        if (_warning) m_EnemyWarning++;
        else 
        {
            if (m_EnemyWarning > 0)
                m_EnemyWarning--;
        }
            
        
        if (m_EnemyWarning > 0) Time.timeScale = 0.2f;
        else Time.timeScale = 1f;
    }

    public void LoadSceneTest()
    {
        SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Single);
    }

    public async UniTask LoadLevelTask()
    {
        if (LevelController.Instance != null)
        {
            InGameManager.Instance.img_Flash.gameObject.SetActive(true);
            InGameManager.Instance.img_Flash.DOFade(1, 0.3f);
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.4f));
            // CamController.Instance.ResetLevel();
            Destroy(LevelController.Instance.gameObject);
            
            SimplePool.Release();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();

            await UniTask.WaitUntil(() => m_LevelLoaded == false);
        }
        // else
        // {
        //     await UniTask.WhenAll(CamController.Instance.CameraIntro(new Vector3(0f, 5f, 0f), 1.5f)); 
        // }

        int level = ProfileManager.GetLevel();
        AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>("Level_" + level.ToString());

        await goHandle;
        
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = goHandle.Result;
            Instantiate(obj).GetComponent<Transform>().parent = InGameManager.Instance.tf_LevelHolder;
        }

        await UniTask.WaitForEndOfFrame();
        
        // if (LevelController.Instance == null)
        // {
            

        // CamController.Instance.CameraIntro(new Vector3(0f, 5f, 0f), 1.5f);

        CamController.Instance.m_CMCam.Follow = LevelController.Instance.tf_CamLookPoint;
        CamController.Instance.m_CMCam.LookAt = LevelController.Instance.tf_CamLookPoint;
    }
}
