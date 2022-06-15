using System;
using System.Collections;
using System.Collections.Generic;
using Exploder.Utils;
using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    [SerializeField] private Transform visualTransform;

    private Transform hitTransform;
    private bool isEnemyShot;
    private float shootingForce;
    private Vector3 direction;
    private Vector3 hitPoint;
    private RaycastHit rayHit;

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
        transform.Translate(direction * shootingForce * Time.deltaTime, Space.World);
    }

    private void CheckDistanceToEnemy()
    {
        float distance = Vector3.Distance(transform.position, hitPoint);
        if(distance <= 0.1 && !isEnemyShot)
        {
            Enemy3 enemy = hitTransform.GetComponentInParent<Enemy3>();
            if (enemy)
            {
                ShootEnemy(hitTransform, enemy);
            }
        }
    }

    private void Rotate()
    {
        visualTransform.Rotate(Vector3.forward, 1200 * Time.deltaTime, Space.Self);
    }

    private void ShootEnemy(Transform hitTransform, Enemy3 enemy)
    {
        isEnemyShot = true;
        Rigidbody shotRB = hitTransform.GetComponent<Rigidbody>();
        enemy.OnEnemyShot(transform.forward, shotRB);
        Helper.DebugLog("Name:" + rayHit.collider.name);
        ExploderSingleton.Instance.ExplodeObject(rayHit.collider.gameObject);
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