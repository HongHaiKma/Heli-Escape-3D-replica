using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BreakableElement : MonoBehaviour, ITrap
{
    public GameObject go_Shatter;
    public GameObject go_Bridge;
    public Collider col_Owner;
    public Collider col_Ground;
    
    public void OnTrigger()
    {
        go_Shatter.SetActive(true);
        go_Bridge.SetActive(false);
        col_Owner.enabled = false;
        col_Ground.enabled = false;
    }

    [Button]
    public void Activate()
    {
        OnTrigger();
    }
}
