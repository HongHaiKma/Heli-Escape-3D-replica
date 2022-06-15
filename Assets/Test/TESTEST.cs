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

        for (int i = 0; i < 7; i++)
        {
            Debug.Log("I: " + i);
            int a = i;
        }
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
        Start();
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
