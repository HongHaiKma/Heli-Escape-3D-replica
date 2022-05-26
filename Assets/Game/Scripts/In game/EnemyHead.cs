using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour, IDamageable
{
    public Enemy m_EnemyOwner;
    
    public void OnHit(Vector3 _pos)
    {
        Debug.Log("Head shotttttttttttt");
        m_EnemyOwner.OnHit(_pos);
    }
}
