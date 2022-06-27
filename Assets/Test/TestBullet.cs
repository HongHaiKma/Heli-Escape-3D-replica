using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [Title("Bullet Collision")]
    public Vector3 m_ExplosionRadius;
    public Transform tf_ExploPivot;
    public float m_ExplosionForce;
    public bool collide = false;
    public float speed;
    public float speed2;
    public Transform tf_Target;
    
    public virtual void FixedUpdate()
    {
        // tf_Owner.position += tf_Owner.forward * m_MoveSpd * Time.fixedDeltaTime;
        // rb_Owner.position += rb_Owner.transform.forward * m_MoveSpd * Time.fixedDeltaTime;
        
        // rb_Owner.MovePosition(rb_Owner.transform.forward * 5f * Time.fixedDeltaTime);


        // if (tf_Target != null)
        // {
        //     tf_Owner.LookAt(tf_Target);
        // }
        
        // rb_Owner.AddRelativeForce(transform.forward * 200f);
        
        // rb_Owner.
        
        // rb_Owner.velocity = tf_Owner.forward * 20f;
        
        // if (collide)
        // {
        //     // transform.position += transform.forward.normalized * Time.deltaTime * speed;
        //     
        //     Vector3 explosionPos = tf_ExploPivot.position;
        //     Collider[] colliders = Physics.OverlapBox(explosionPos, m_ExplosionRadius);
        //     foreach (Collider hit in colliders)
        //     {
        //         Helper.DebugLog("Name: " + hit.name);
        //     }
        // }

        if (collide)
        {
            // GetComponent<Rigidbody>().DOLookAt(tf_Target.position, 0f);
            Quaternion targetRotation = Quaternion.LookRotation(tf_Target.position - transform.position);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.fixedDeltaTime);
            
            // transform.LookAt(tf_Target);
            GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward.normalized * Time.deltaTime * speed2);  
            GetComponent<Rigidbody>().MoveRotation(targetRotation);  
        }

        // m_FlyingTime += Time.fixedDeltaTime;
        //
        // if (m_FlyingTime >= m_FlyingTimeMax)
        // {
        //     PrefabManager.Instance.DespawnPool(gameObject);
        //     // Helper.DebugLog("BBBBBBBBBBBB");
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.tag.Equals("Target"))
        // {
            Helper.DebugLog("AAAAAAAAAA");
        // }
    }

    [Button]
    public void Fire()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // private void OnValidate()
    // {
    //     
    // }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(tf_ExploPivot.position, m_ExplosionRadius);
    }
#endif
}