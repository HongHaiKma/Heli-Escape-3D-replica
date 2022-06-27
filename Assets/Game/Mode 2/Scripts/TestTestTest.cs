using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestTestTest : MonoBehaviour
{
    public Vector3 origin;
    public bool a = false;

    private void OnValidate()
    {
        if (a)
        {
            transform.rotation = new Quaternion(0f, -180f, 0f, 0f);
        }
    }
}
