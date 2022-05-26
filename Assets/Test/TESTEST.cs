using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class TESTEST : MonoBehaviour
{
    public PlayableDirector track_Test;

    private void Start()
    {
        Test();
    }

    private void Update()
    {
        if (Helper.GetKeyDown(KeyCode.A))
        {
            track_Test.Play(); 
        }
    }

    async UniTask Test()
    {
        await UniTask.WhenAll(Test1(), Test2());
        await UniTask.Delay(2000);
        Debug.Log("Test");
    }
    
    async UniTask Test1()
    {
        await UniTask.Delay(2000);
        Debug.Log("Test1");
    }
    
    async UniTask Test2()
    {
        await UniTask.Delay(3000);
        Debug.Log("Test2");
    }
}
