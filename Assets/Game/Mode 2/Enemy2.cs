using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public Transform tf_Owner;
    public bool isActive;

    private void OnEnable()
    {
        isActive = false;
    }
}
