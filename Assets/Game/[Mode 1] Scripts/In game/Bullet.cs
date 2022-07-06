using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Sirenix.OdinInspector;
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

    [FormerlySerializedAs("tf_Onwer")]
    [Header("---Components---")]
    public Transform tf_Owner;
    public Collider col_Onwer;
    public Rigidbody rb_Owner;
    public Vector3 v3_CollisionPoint;


    [Header("---Movements---")]
    public float m_MoveSpd;
    public float m_MoveSpdVelocity;
    public float m_FlyingTime;
    public float m_FlyingTimeMax;
    public bool m_Collided;
    public Transform tf_Target;

    [Title("Bullet Collision")]
    public Vector3 m_ExplosionRadius;
    public Transform tf_ExploPivot;
    public float m_ExplosionForce;
    public bool collide = false;

    // public void Update()
    // {
    //     if (collide)
    //     {
    //         Vector3 explosionPos = tf_ExploPivot.position;
    //         Collider[] colliders = Physics.OverlapBox(explosionPos, m_ExplosionRadius);
    //         foreach (Collider hit in colliders)
    //         {
    //             // Helper.DebugLog("Name: " + hit.name);
    //             ITrap iTrap = hit.GetComponent<ITrap>();
    //
    //             if (iTrap != null)
    //             {
    //                 iTrap.OnTrigger();
    //             }
    //         }
    //     }
    // }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(tf_ExploPivot.position, m_ExplosionRadius);
    }
#endif


    public virtual void FixedUpdate()
    {
        // if (collide)
        // {
        //     Vector3 explosionPos = tf_ExploPivot.position;
        //     Collider[] colliders = Physics.OverlapBox(explosionPos, m_ExplosionRadius);
        //     foreach (Collider hit in colliders)
        //     {
        //         // Helper.DebugLog("Name: " + hit.name);
        //         ITrap iTrap = hit.GetComponent<ITrap>();
        //
        //         if (iTrap != null)
        //         {
        //             iTrap.OnTrigger();
        //         }
        //     }
        // }

        rb_Owner.velocity = transform.forward * m_MoveSpdVelocity * Time.fixedDeltaTime;

        m_FlyingTime += Time.fixedDeltaTime;

        if (m_FlyingTime >= m_FlyingTimeMax)
        {
            PrefabManager.Instance.DespawnPool(gameObject);
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

        // rb_Owner.velocity = transform.forward * m_MoveSpdVelocity * Time.fixedDeltaTime;

        // OnChangeSpeed(Physics.autoSimulation);
        EventManager1<bool>.AddListener(GameEvent.SLOWMOTION, OnChangeSpeed);
        // EventManager1<int>.AddListener(GameEvent.DESTROY_SCORE_LINE, Event_SCORE_LINE_PICK);
    }

    private void OnDisable()
    {
        EventManager1<bool>.RemoveListener(GameEvent.SLOWMOTION, OnChangeSpeed);
    }

    private void OnDestroy()
    {
        EventManager1<bool>.RemoveListener(GameEvent.SLOWMOTION, OnChangeSpeed);
    }

    public void OnChangeSpeed(bool _slowmotion)
    {
        if (_slowmotion)
        {
            m_MoveSpdVelocity = 1500f;
        }
        else
        {
            m_MoveSpdVelocity = 2500f;
        }
    }

    public void Fire(Vector3 _lookAt, Transform _tfTarget)
    {
        tf_Target = _tfTarget;
        tf_Owner.LookAt(_lookAt);
        // rb_Owner.AddRelativeForce(transform.forward * 2300f);
        // rb_Owner.AddForce(transform.forward * m_MoveSpd);
        // rb_Owner.velocity = tf_Owner.forward * 5f;
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     IBreakable2 iBreak = other.gameObject.GetComponent<IBreakable2>();
    //
    //     if (iBreak != null)
    //     {
    //         iBreak.OnTrigger(gameObject);
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        // IDamageable iDMG = collision.gameObject.GetComponent<IDamageable>();
        // if (iDMG != null)
        // {
        //     iDMG.OnHit(tf_Owner.position);
        // }

        // Helper.DebugLog("AAAAAAAAAAAAAAA");

        string colTag = collision.gameObject.tag;
        if (colTag.Equals("Furniture"))
        {
            EventManager1<bool>.CallEvent(GameEvent.MODE_2_SHOOTER_SHOT, false);
            PrefabManager.Instance.SpawnVFXPool("BulletHit", collision.contacts[0].point);
            PrefabManager.Instance.DespawnPool(gameObject);
            return;
        }

        IBreakable2 iBreak = collision.gameObject.GetComponent<IBreakable2>();

        if (iBreak != null)
        {
            EventManager1<bool>.CallEvent(GameEvent.MODE_2_SHOOTER_SHOT, false);
            rb_Owner.velocity = transform.forward * m_MoveSpdVelocity * Time.fixedDeltaTime;
            iBreak.OnTrigger(gameObject);
        }


        IEnemy2 iEnemy2 = collision.gameObject.GetComponent<IEnemy2>();

        if (iEnemy2 != null)
        {
            iEnemy2.OnHit(tf_Owner.position);
            PrefabManager.Instance.DespawnPool(gameObject);
        }

        IDamageable iDamage = collision.gameObject.GetComponent<IDamageable>();

        if (iDamage != null)
        {
            EventManager1<bool>.CallEvent(GameEvent.MODE_2_SHOOTER_SHOT, false);
            Helper.DebugLog("Name: " + collision.gameObject.name);
            iDamage.OnHit(tf_Owner.position);
            PrefabManager.Instance.DespawnPool(gameObject);
        }

        // if (collision.gameObject.tag.Equals("Shooter"))
        // {
        //     SceneManager.LoadScene("PlaySceneMode2");
        // }
    }
}