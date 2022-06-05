using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class TestFollow : Singleton<TestFollow>
{
    public bool a;
    public bool b;

    public AIPath ai;
    public Transform tf_Target;

    private void Update()
    {
        ai.destination = tf_Target.position;
    }
}
