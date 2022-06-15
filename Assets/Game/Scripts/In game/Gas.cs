using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour, IDamageable
{
    public Vector3 m_ExplosionRadius;
    public Transform tf_ExploPivot;
    public float m_ExplosionForce;
    
    public void Explode()
    {
        Vector3 explosionPos = tf_ExploPivot.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 6);
        foreach (Collider hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            
            if (enemy != null)
            {
                enemy.OnHit(enemy.tf_Owner.position);
                enemy.rb_Owner.AddExplosionForce(m_ExplosionForce, explosionPos, 5f, 5f);
            }
        }
    }
    
    public void OnHit(Vector3 _pos)
    { 
        Explode();
        Destroy(gameObject);
        PrefabManager.Instance.SpawnVFXPool("VFX_5", _pos);
    }
    
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(tf_ExploPivot.position, m_ExplosionRadius);
    }
#endif
}
