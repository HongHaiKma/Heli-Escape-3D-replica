using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

    private void OnEnable()
    {
        m_CurFloor = 0;
        CamController2.Instance.ActivateFloor(m_Floors[m_CurFloor]);
        tf_PivotFollower.DOKill();
        tf_PivotFollower.DORotate(new Vector3(0f, -360f, 0f), m_RotateSpeed, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        tf_PivotFollower.position = tf_Pivots[m_CurFloor].position;
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
                InGameManager2.Instance.go_PopupWin.SetActive(true);
                return;
            }
            
            tf_PivotFollower.DOLocalMove(tf_Pivots[m_CurFloor].position, 2f).OnStart(() => Time.timeScale = 1f).OnComplete(() =>
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
    }
}
