using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Bullet : MonoBehaviour
{
    [Header("---Charcteristics---")]
    public BigNumber m_Dmg;
    // public TEAM m_Team;

    [FormerlySerializedAs("tf_Onwer")] [Header("---Components---")]
    public Transform tf_Owner;
    public Collider col_Onwer;
    public Rigidbody rb_Owner;
    public Vector3 v3_CollisionPoint;


    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_FlyingTime;
    public float m_FlyingTimeMax;
    public bool m_Collided;
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

        m_FlyingTime += Time.fixedDeltaTime;

        if (m_FlyingTime >= m_FlyingTimeMax)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
            // Helper.DebugLog("BBBBBBBBBBBB");
        }
    }

    public virtual void OnEnable()
    {
        tf_Target = null;
        rb_Owner.velocity = Vector3.zero;
        rb_Owner.drag = 0f;
        rb_Owner.angularDrag = 0f;
        rb_Owner.angularVelocity = Vector3.zero; 
        m_Collided = false;
        m_FlyingTime = 0f;
    }

    public void Fire(Vector3 _lookAt, Transform _tfTarget)
    {
        tf_Target = _tfTarget;
        tf_Owner.LookAt(_lookAt);
        // rb_Owner.AddRelativeForce(transform.forward * 2300f);
        rb_Owner.AddForce(transform.forward * 6000f);
        // rb_Owner.velocity = tf_Owner.forward * 5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable iDMG = collision.gameObject.GetComponent<IDamageable>();
        if (iDMG != null)
        {
            iDMG.OnHit(tf_Owner.position);
        }
        
        if (collision.gameObject.tag.Equals("Shooter"))
        {
            SceneManager.LoadScene("PlaySceneMode2");
        }

        PrefabManager.Instance.DespawnPool(gameObject);
    }
}

public interface IDamageable
{
    void OnHit(Vector3 _pos);
}