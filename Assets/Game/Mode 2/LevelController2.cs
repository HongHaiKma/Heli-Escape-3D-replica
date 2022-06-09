using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelController2 : Singleton<LevelController2>
{
    public Transform tf_PivotFollower;
    public List<Transform> tf_Pivots;
    public int m_CurFloor;
    public float m_RotateSpeed;
    public List<Floor> m_Floors;

    private void OnEnable()
    {
        // m_Floors.
        tf_PivotFollower.DOKill();
        tf_PivotFollower.DORotate(new Vector3(0f, -360f, 0f), m_RotateSpeed, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        m_CurFloor = 0;
        tf_PivotFollower.position = tf_Pivots[m_CurFloor].position;
    }

    private void Update()
    {
        // tf_PivotFollower.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        // if (Helper.GetKeyDown(KeyCode.A))
        // {
        //     m_CurFloor++;
        //     tf_PivotFollower.DOLocalMove(tf_Pivots[m_CurFloor].position, 2f);
        // }
    }
}
