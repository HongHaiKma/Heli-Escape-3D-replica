using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICaution : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }
}
