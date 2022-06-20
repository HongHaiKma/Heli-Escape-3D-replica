using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour, ITriggerble
{
    public Collider col_Owner;
    public Vector3 v3_Offset;
    public float f_Time;
    
    public void OnTrigger()
    {
        col_Owner.enabled = false;

        CamController.Instance.CameraOffset(v3_Offset, f_Time);
    }
}
