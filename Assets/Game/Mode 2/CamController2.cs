using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamController2 : Singleton<CamController2>
{
    public List<Enemy2> m_Enemies;
    public bool slow = false;
    public Transform tf_FirePivot;
    public int m_IgnoreLayer = ~(1 << 15);

    private void OnEnable()
    {
        // ResetLevel();
    }

    public void ResetLevel()
    {
        Time.timeScale = 1;
        slow = false;
        m_Enemies.Clear();
    }

    public void ActivateFloor(Floor _floor)
    {
        ResetLevel();
        m_Enemies = _floor.m_Enemies;
    }

    private void Update()
    {
        if (m_Enemies.Count > 0)
        {
            Time.timeScale = m_Enemies.Any(IsSlow) ? Time.timeScale = 0.4f : Time.timeScale = 1f;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out  hitInfo, Mathf.Infinity, m_IgnoreLayer))
            {
                var col = hitInfo.collider.GetComponent<Collider>();
                GameObject go = hitInfo.transform.gameObject;
                // if (col != null)
                if (go != null)
                {
                    // if (col.tag.Equals("Enemy") || col.tag.Equals("EnemyHead") )
                    if (go.tag.Equals("Enemy") || go.tag.Equals("EnemyHead"))
                    {
                        Transform trans = hitInfo.collider.GetComponent<Transform>();
                        // tf_LookAimIK.position = hitInfo.point;
                        Shoot(hitInfo.point, trans);
                    }
                    // else if (col.tag.Equals("Ground") || col.tag.Equals("Gas") || col.tag.Equals("Untagged"))
                    else if (go.tag.Equals("Ground") || go.tag.Equals("Gas") || go.tag.Equals("Untagged"))
                    {
                        // tf_LookAimIK.position = hitInfo.point;
                        Shoot(hitInfo.point, null);
                    }
                }
            }
        } 
    }
    
    public void Shoot(Vector3 _lookAt, Transform _tfEnemy)
    {
        GameObject go = PrefabManager.Instance.SpawnVFXPool("VFX_2", tf_FirePivot.position);
        go.transform.LookAt(_lookAt);
        go.transform.parent = tf_FirePivot;
        PrefabManager.Instance.SpawnBulletPool("Bullet1", tf_FirePivot.position).GetComponent<Bullet>().Fire(_lookAt, _tfEnemy);
    }

    public bool IsSlow(Enemy2 _enemy)
    {
        Vector3 targetDir = transform.position - _enemy.tf_Owner.position;
        targetDir = targetDir.normalized;
       
        float dot = Vector3.Dot(targetDir, _enemy.tf_Owner.forward);
        float angle = Mathf.Acos( dot ) * Mathf.Rad2Deg;

        if (angle < 60f)
        {
            _enemy.SetInRange(true);
            return true;
        }

        _enemy.SetInRange(false);
        return false;
    }
}
