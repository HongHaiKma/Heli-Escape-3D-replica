using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Exploder;
using Exploder.Utils;
using UnityEngine;

public class Breakable2 : MonoBehaviour, IBreakable2
{
    public bool m_Activated;
    public Collider col_Owner;
    public MeshRenderer mesh_Owner;
    public Rigidbody rb_Owner;
    public ExploderObject m_ExploderObject;
    public GameObject go_Pieces;

    public void ActivateFloor()
    {
        m_Activated = true;
        mesh_Owner.material.DOColor(Helper.ConvertColor(255f, 255f, 255f, 90), "_Color", 0.5f);
        rb_Owner.isKinematic = false;
    }
    
    public void OnTrigger(GameObject _goBullet)
    {
        if (m_Activated)
        {
            col_Owner.enabled = false;
            mesh_Owner.material.DOColor(Helper.ConvertColor(255f, 255f, 255f, 15), "_Color", 0.5f);
            GetComponent<Rigidbody>().useGravity = true;
            // go_Pieces.
            
            // ExploderSingleton.Instance.ExplodeObject(gameObject);
            m_ExploderObject.ExplodeObject(gameObject);
            // ExploderSingleton.Instance.ExplodeObject(gameObject);
            // PrefabManager.Instance.DespawnPool(gameObject);
        }
        else
        {
            PrefabManager.Instance.DespawnPool(_goBullet);
        }
    }
}
