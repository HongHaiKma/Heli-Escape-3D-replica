using System;
using System.Collections;
using System.Collections.Generic;
using Exploder.Utils;
using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    [SerializeField] private Transform visualTransform;

    public Transform hitTransform;
    public bool isEnemyShot;
    public float shootingForce;
    public Vector3 direction;
    public Vector3 hitPoint;
    public RaycastHit rayHit;

    public void Launch(float shootingForce, Transform hitTransform, Vector3 hitPoint, RaycastHit hit)
    {
        direction = (hitPoint - transform.position).normalized;
        isEnemyShot = false;
        this.hitTransform = hitTransform;
        this.shootingForce = shootingForce;
        this.hitPoint = hitPoint;
        this.rayHit = hit;
    }

    private void Update()
    {
        Move();
        Rotate();
        CheckDistanceToEnemy();
    }

    private void Move()
    {
        transform.LookAt(hitPoint);
        transform.Translate(direction * shootingForce * Time.deltaTime, Space.World);
    }

    private void CheckDistanceToEnemy()
    {
        float distance = Vector3.Distance(transform.position, hitPoint);
        if(distance <= 0.25f && !isEnemyShot)
        {
            IBodyPart enemy = hitTransform.GetComponent<IBodyPart>();
            if (enemy != null)
            {
                ShootEnemy(hitTransform, enemy);
            }
        }
    }

    private void Rotate()
    {
        visualTransform.Rotate(Vector3.forward, 1200 * Time.deltaTime, Space.Self);
    }

    private void ShootEnemy(Transform hitTransform, IBodyPart enemy)
    {
        isEnemyShot = true;
        Rigidbody shotRB = hitTransform.GetComponent<Rigidbody>();
        PrefabManager.Instance.SpawnVFXPool("VFX_4", transform.position);
        enemy.OnHit();
 
        // ExploderSingleton.Instance.ExplodeObject(rayHit.collider.gameObject);
    }

    public float GetBulletSpeed()
    {
        return shootingForce;
    }

	internal Transform GetHitEnemyTransform()
	{
        return hitTransform;
	}
}