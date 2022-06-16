using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class RemoveAllColliders : MonoBehaviour
{
    [Button]
    public void RemoveAllColliderss()
    {
        var allColliders = GetComponentsInChildren<Collider>();
 
        foreach(var childCollider in allColliders) DestroyImmediate(childCollider);
        
        var allJoints = GetComponentsInChildren<Joint>();
 
        foreach(var childRigid in allJoints) DestroyImmediate(childRigid);
        
        var allRigids = GetComponentsInChildren<Rigidbody>();

        foreach (var childRigid in allRigids) DestroyImmediate(childRigid);

        var go = GetComponentsInChildren<Component>();
        
        foreach(var g in go) GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g.gameObject);
    }
}
