using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas2 : MonoBehaviour, IDamageable
{
    public void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 6);
        foreach (Collider hit in colliders)
        {
            Enemy2 enemy = hit.GetComponent<Enemy2>();
            
            if (enemy != null)
            {
                // enemy.DoRagdoll(explosionPos);
                
                // enemy.m_StateMachine.ChangeState(DeathState.Instance);

                // Rigidbody rb = GetComponent<Rigidbody>();
                //
                // if (rb != null) rb.AddExplosionForce(70f, explosionPos, 10f, 2f);
            }
        }
    }
    
    public void OnHit(Vector3 _pos)
    { 
        Explode();
        Destroy(gameObject);
        PrefabManager.Instance.SpawnVFXPool("VFX_5", _pos);
    }
}