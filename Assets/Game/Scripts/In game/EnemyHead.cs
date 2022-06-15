using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour, IDamageable
{
    public Enemy m_EnemyOwner;
    public Collider col_Owner;

    private void OnEnable()
    {
        col_Owner.enabled = true;
    }

    public void OnHit(Vector3 _pos)
    {
        if (GameManager.Instance.m_GameLoop == GameLoop.WaitEndGame)
        {
            PrefabManager.Instance.SpawnVFXPool("UIHeadshot", Vector3.zero).GetComponent<UIDamage>().Fly(_pos, UIIngame.Instance.tf_MainCanvas);
        }
        col_Owner.enabled = false;
        // Debug.Log("Head shotttttttttttt");
        m_EnemyOwner.OnHit(_pos);
    }
}
