using System;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2;
using Exploder.Utils;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShootingController : MonoBehaviour
{
    [SerializeField] BulletTimeController bulletTimeController;
    [FormerlySerializedAs("bulletPrefab")] [SerializeField] Bullet3 bullet3Prefab;
    [SerializeField] Transform bulletSpawnTransform;
    [SerializeField] Scope scope;
    [SerializeField] private float shootingForce;
    [SerializeField] private float minDistanceToPlayAnimation;
    public bool isScopeEnabled = false;
    public bool isShooting = false;
    private float scrollInput = 0f;
    private bool wasScopeOn;

    public TouchTrackPad m_TrackPad;

    private void Update()
    {
        GetInput();
    }

    private void HandleShooting()
    {
        if (isShooting)
            Shoot();
    }

    private void Shoot()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit))
        {
            // Enemy3 controller = hit.collider.GetComponentInParent<Enemy3>();
            Enemy3 IDamage = hit.collider.GetComponentInParent<Enemy3>();
            Vector3 direction = hit.point - bulletSpawnTransform.position;
            if (IDamage != null)
            {
                if (IDamage.m_Health - 1 <= 0) //LOGIC TRIGGER BULLET TIME
                {
                    // controller.StopAnimation();
                    Bullet3 bullet3Instance = Instantiate(bullet3Prefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
                    bullet3Instance.Launch(shootingForce, hit.collider.transform, hit.point, hit);
                    bulletTimeController.StartSequence(bullet3Instance, hit.point);
                    // ExploderSingleton.Instance.ExplodeObject(hit.collider.gameObject);
                }
                else
                {
                    // controller.OnEnemyShot(direction, hit.collider.GetComponent<Rigidbody>());
                    IDamage.OnHit(false);
                    // ExploderSingleton.Instance.ExplodeObject(hit.collider.gameObject);
                }
            }       
        }
    }

    private void HandleScope()
    {
        scope.ChangeScopeFOV();
        if (!wasScopeOn)
            scope.ResetScopeFOV();
        scope.SetScopeFlag(isScopeEnabled);
        wasScopeOn = isScopeEnabled;
    }

    private void GetInput()
    {
        if (m_TrackPad.JustPressed())
        {
            scope.ChangeScopeFOV();
            scope.SetScopeFlag(true);
        }

        if (m_TrackPad.JustReleased())
        {
            scope.SetScopeFlag(false);
            Shoot();
        }
    }
}
