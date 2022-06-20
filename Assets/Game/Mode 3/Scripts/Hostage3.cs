using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Hostage3 : MonoBehaviour, IDamageable
{
    public Transform tf_Owner;

    public void OnHit(Vector3 _pos)
    {
        PrefabManager.Instance.SpawnVFXPool("VFX_4", tf_Owner.position);
        Helper.DebugLog("DIEEEEEEEEEEEEEEEEEEEEEE");
        LevelController3.Instance.m_Hostages.Remove(this);
        PrefabManager.Instance.DespawnPool(gameObject);
    }

    [Button]
    public void SetTransform()
    {
        tf_Owner = transform;
    }
}
