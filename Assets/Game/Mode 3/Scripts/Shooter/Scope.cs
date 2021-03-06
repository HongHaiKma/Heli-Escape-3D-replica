using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Scope : MonoBehaviour
{
    private const string SCOPE = "Scope";
    [SerializeField] GameObject postProcessingVolume;
    [SerializeField] private Camera scopeCamera;
    [SerializeField] private float minScopeFOV;
    [SerializeField] private float maxScopeFOV;
    [SerializeField] private float scopeFOVChangeSpeed;

    public Animator animator;

    private void OnEnable()
    {
        animator.SetTrigger("Scope Off");
        SetScopeFlag(false);
    }

    public void SetScopeFlag(bool isScopeOn)
    {
        animator.SetBool(SCOPE, isScopeOn);
    }

    public void ChangeScopeFOV(float delta)
    {
        Helper.DebugLog("Delta: " + delta);
        scopeCamera.fieldOfView += delta * Time.deltaTime * scopeFOVChangeSpeed;
        scopeCamera.fieldOfView = Mathf.Clamp(scopeCamera.fieldOfView, minScopeFOV, maxScopeFOV);
    }

    public void ChangeScopeFOV()
    {
        scopeCamera.fieldOfView = 10f;
        scopeCamera.fieldOfView = Mathf.Clamp(scopeCamera.fieldOfView, minScopeFOV, maxScopeFOV);
    }

    internal void ResetScopeFOV()
    {
        scopeCamera.fieldOfView = maxScopeFOV;
    }

    public void EnablePostProcessing()
    {
        // postProcessingVolume.gameObject.SetActive(true);
    }

    public void DisablePostProcessing()
    {
        // postProcessingVolume.gameObject.SetActive(false);
    }

    public float GetZoomPrc()
    {
        float range = Mathf.Abs(maxScopeFOV - minScopeFOV);
        float currentZoonDelta = scopeCamera.fieldOfView - minScopeFOV;
        return currentZoonDelta / range;

    }
}
