using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class RemoveAllColliders : MonoBehaviour
{
    public bool col, rigid, joint; 
    
    [Button]
    public void RemoveAllColliderss()
    {
        if (col)
        {
            var allColliders = GetComponentsInChildren<Collider>();
 
            foreach(var childCollider in allColliders) DestroyImmediate(childCollider); 
        }

        if (rigid)
        {
            var allJoints = GetComponentsInChildren<Joint>();
 
            foreach(var childRigid in allJoints) DestroyImmediate(childRigid);  
        }

        if (joint)
        {
            var allRigids = GetComponentsInChildren<Rigidbody>();

            foreach (var childRigid in allRigids) DestroyImmediate(childRigid);
        }

        // var go = GetComponentsInChildren<Component>();
        //
        // foreach(var g in go) GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g.gameObject);
    }
}
