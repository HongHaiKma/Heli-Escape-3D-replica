using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    public Transform tf_Owner;
    protected float m_LifeTime;
    public float m_LifeTimeMax = 2;
    public Transform tf_Follow;

    public virtual void OnEnable()
    {
        m_LifeTime = 0;

        StartCoroutine(IEUpdate());
    }

    public void SetFollow(Transform _tfFollow)
    {
        tf_Follow = _tfFollow;
    }

    protected IEnumerator IEUpdate()
    {
        while (true)
        {
            yield return Yielders.EndOfFrame;
            m_LifeTime += Time.deltaTime;

            if (tf_Follow != null)
            {
                tf_Owner.position = tf_Follow.position;
            }

            if (m_LifeTime >= m_LifeTimeMax)
            {
                Deactivate();
            }
        }
    }

    // private void Update()
    // {
    //     m_LifeTime += Time.deltaTime;

    //     if (tf_Follow != null)
    //     {
    //         tf_Owner.position = tf_Follow.position;
    //     }

    //     if (m_LifeTime >= m_LifeTimeMax)
    //     {
    //         Deactivate();
    //     }
    // }

    public void Deactivate()
    {
        tf_Follow = null;
        PrefabManager.Instance.DespawnPool(gameObject);
    }
}
