using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class TestFollow : Singleton<TestFollow>
{
    // public bool a;
    // public bool b;
    //
    // public AIPath ai;
    // public Transform tf_Target;
    //
    // private void Update()
    // {
    //     ai.destination = tf_Target.position;
    // }

    private void Update()
    {
        if (Helper.GetKeyDown(KeyCode.A))
        {
            GraphNode nearestNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
            if (nearestNode != null)
            {
              // if (nearestNode.Walkable)
              //         {
              //             Int3 a = new Int3();
              //             Vector3 b = (Vector3)a;
              //                            
              //             // m_AIPath.destination = b;
              //             // transform.position = (Vector3)nearestNode.position;
              //             
              //             
              //         }  

              Helper.DebugLog("Node: " + nearestNode.ClosestPointOnNode(transform.position));
            }
        }
    }

    // GraphNode nearestNode = AstarPath.active.GetNearest(tf_Owner.position, NNConstraint.Default).node;
    // if (nearestNode != null)
    // {
    //   if (nearestNode.Walkable)
    //           {
    //               Int3 a = new Int3();
    //               Vector3 b = (Vector3)a;
    //                              
    //               // m_AIPath.destination = b;
    //               rb_Owner.position = (Vector3)nearestNode.position;
    //           }  
    // }
}
