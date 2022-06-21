using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class RemoveAllColliders : MonoBehaviour
{
    public bool col, rigid, joint, setLayerByScript; 
    
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

        if (setLayerByScript)
        {
            var bodyParts = GetComponentsInChildren<BodyPart3>();
            
            foreach (var bodyPart in bodyParts)
            {
                if (bodyPart != null)
                {
                    bodyPart.gameObject.layer = LayerMask.NameToLayer("EnemyBody");
                }
            }
        }

        // var go = GetComponentsInChildren<Component>();
        //
        // foreach(var g in go) GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g.gameObject);
    }
}
