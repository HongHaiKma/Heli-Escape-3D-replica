using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestCam : MonoBehaviour
{
    public Transform tf_A;
    public Transform tf_B;
    public Transform tf_Test;
    public bool m_Rotating;

    private void Start()
    {
        m_Rotating = false;
        tf_Test = tf_A;
        transform.LookAt(tf_Test);
    }

    private void Update()
    {
        if (!m_Rotating)
        {
            transform.LookAt(tf_Test);  
        }

        if (Helper.GetKeyDown(KeyCode.A))
        {
            CameraOffset(tf_A.position, 2f);
        }
    }

    public async UniTask CameraOffset(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        m_Rotating = true;
        while (time < duration)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(tf_B.position - transform.position);
 
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, time / duration);
            // transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            // transform.LookAt(tf_B);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        tf_Test = tf_B;
        m_Rotating = false;
        transform.LookAt(tf_Test);
    }
}
