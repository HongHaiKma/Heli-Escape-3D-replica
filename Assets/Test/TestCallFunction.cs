using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCallFunction : MonoBehaviour
{
    public float m_Health;
    public List<Transform> tf_Transform;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TestFunc();
        }
    }

    public void TestFunc()
    {
        for (int i = 0; i < tf_Transform.Count; i++)
        {
            Helper.DebugLog("Position: " + tf_Transform[i].position);
        }
    }
}
