using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class CameraDepthEnable : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}
