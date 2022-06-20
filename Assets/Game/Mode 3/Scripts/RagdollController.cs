using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Rigidbody[] _rigidbodies;
    public Rigidbody[] rigidbodies
    {
        get
        {
            if (_rigidbodies == null)
                _rigidbodies = GetComponentsInChildren<Rigidbody>();
            return _rigidbodies;
        }       
    }
    private void Awake()
    {
        // if (_rigidbodies == null)
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
        // return _rigidbodies;
        DisableRagdoll();
    }
    private void DisableRagdoll()
    {
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.None;

        }
    }
    public void EnableRagdoll()
    {
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    private void Update()
    {
        DebugRagdoll();
    }

    private void DebugRagdoll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EnableRagdoll();
    }
}
