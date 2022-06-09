using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestTestTest : MonoBehaviour
{
    public List<int> a;

    private void Update()
    {
        if (Helper.GetKeyDown(KeyCode.A))
        {
            if (a.All(CheckA))
            {
                Helper.DebugLog("AAAAAAAAAAA");
            }
        }
    }

    public bool CheckA(int a)
    {
        if (a < 3)
        {
            return true;
        }

        return false;
    }
}
