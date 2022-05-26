using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUI : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(CamController.Instance.tf_Owner);
        transform.Rotate(0f, 180f, 0f);
    }
}
