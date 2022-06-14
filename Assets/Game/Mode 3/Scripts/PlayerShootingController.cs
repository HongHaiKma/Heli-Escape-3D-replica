using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool isScopeEnabled = false;
    private float scrollInput = 0f;
    private bool isShooting = false;
    private bool wasScopeOn;

    private void Update()
    {
        GetInput();
        HandleScope();
        HandleShooting();
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
            Enemy3 controller = hit.collider.GetComponentInParent<Enemy3>();
            Vector3 direction = hit.point - bulletSpawnTransform.position;
            if (controller)
            {
                if (direction.magnitude >= minDistanceToPlayAnimation) //LOGIC TRIGGER BULLET TIME
                {
                    controller.StopAnimation();
                    Bullet3 bullet3Instance = Instantiate(bullet3Prefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
                    bullet3Instance.Launch(shootingForce, hit.collider.transform, hit.point);
                    bulletTimeController.StartSequence(bullet3Instance, hit.point);
                }
                else
                {
                    controller.OnEnemyShot(direction, hit.collider.GetComponent<Rigidbody>());
                }
            }       
        }
    }

    private void HandleScope()
    {
        // scope.ChangeScopeFOV(scrollInput);
        scope.ChangeScopeFOV();
        if (!wasScopeOn)
            scope.ResetScopeFOV();
        scope.SetScopeFlag(isScopeEnabled);
        wasScopeOn = isScopeEnabled;
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(1))
            isScopeEnabled = !isScopeEnabled;
        isShooting = Input.GetMouseButtonDown(0) && isScopeEnabled;
        scrollInput = Input.mouseScrollDelta.y;
    }
}
