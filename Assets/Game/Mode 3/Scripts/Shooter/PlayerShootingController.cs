﻿using System;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2;
using Cysharp.Threading.Tasks;
using Exploder.Utils;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShootingController : MonoBehaviour
{
    public BulletTimeController bulletTimeController;
    public Transform bulletSpawnTransform;
    public Scope scope;
    public float shootingForce;
    public float minDistanceToPlayAnimation;
    public bool isScopeEnabled = false;
    public bool isShooting = false;
    private float scrollInput = 0f;
    private bool wasScopeOn;

    public TouchTrackPad m_TrackPad;

    private void Update()
    {
        GetInput();
    }

    private async UniTask Shoot()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit))
        {
            // Enemy3 controller = hit.collider.GetComponentInParent<Enemy3>();
            IBodyPart IBodyPart = hit.collider.GetComponent<IBodyPart>();
            Vector3 direction = hit.point - bulletSpawnTransform.position;
            if (IBodyPart != null)
            {
                if (IBodyPart.OnCanSlowmotion()) //LOGIC TRIGGER BULLET TIME
                {
                    // controller.StopAnimation();
                    // Bullet3 bullet3Instance = Instantiate(bullet3Prefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
                    Bullet3 bullet3Instance = PrefabManager.Instance.SpawnBulletPool("Bullet", bulletSpawnTransform.position, bulletSpawnTransform.rotation).GetComponent<Bullet3>();
                    await UniTask.WaitUntil(() => bullet3Instance.isActiveAndEnabled == true);
                    bullet3Instance.Launch(shootingForce, hit.collider.transform, hit.point, hit);
                    if (bullet3Instance == null)
                    {
                        Helper.DebugLog("NULLLLLLLLLLLLLLLLLLLL");
                    }
                    bulletTimeController.StartSequence(bullet3Instance, hit.point);
                }
                else
                {
                    // controller.OnEnemyShot(direction, hit.collider.GetComponent<Rigidbody>());
                    IBodyPart.OnHit();
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
