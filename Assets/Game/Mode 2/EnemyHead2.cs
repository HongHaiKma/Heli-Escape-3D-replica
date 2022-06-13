using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead2 : MonoBehaviour, IDamageable
{
    public Enemy2 m_EnemyOwner;
    public Collider col_Owner;
    public Enemy2 m_Enemy;

    private void OnEnable()
    {
        col_Owner.enabled = true;
    }

    public void OnHit(Vector3 _pos)
    {
        if (!m_Enemy.IsState(DeathState2.Instance))
        {
            // if (GameManager.Instance.m_GameLoop == GameLoop.WaitEndGame)
                    // {
                    Helper.DebugLog("HEADDDDDDDDD");
                    PrefabManager.Instance.SpawnVFXPool("UIHeadshot2", Vector3.zero).GetComponent<UIDamage>().Fly(_pos);
                    // }
                    col_Owner.enabled = false;
                    // Debug.Log("Head shotttttttttttt");
                    m_EnemyOwner.OnHit(_pos);
        }
    }
}
