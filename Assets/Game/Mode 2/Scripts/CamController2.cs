using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;

public class CamController2 : Singleton<CamController2>
{
    public List<Enemy2> m_Enemies;
    public bool slow = false;
    public Transform tf_FirePivot;
    public int m_IgnoreLayer = ~(1 << 15 | 1 << 3);

    public Transform tf_HeliHolder;
    public Transform tf_MainCamera;

    public CinemachineCameraOffset m_CMCamOffset;
    public CinemachineVirtualCamera m_CMCam;
    public bool IsZooming;

    [Title("Gun")]
    public Gun2 m_GunIngame;
    public Transform tf_GunHolder;
    public GunInventoryConfig m_GunInventoryConfig;

    private void OnEnable()
    {
        // ResetLevel();
        int curGunMode2 = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode2);
        var gunInvent = m_GunInventoryConfig.m_GunItem.Find(x => x.m_ID == curGunMode2);

        SpawnGun(gunInvent);

        tf_HeliHolder.SetParent(null);
        Physics.autoSimulation = false;
        IsZooming = false;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Physics.autoSimulation = true;
    }

    public void SpawnGun(GunInventoryItem _gunInvent)
    {
        if (m_GunIngame != null)
        {
            PrefabManager.Instance.DespawnPool(m_GunIngame.gameObject);
            m_GunIngame = null;
        }

        Transform gun = GameObject.Instantiate(_gunInvent.go_UIPrefabInGame, tf_GunHolder).GetComponent<Transform>();
        m_GunIngame = gun.GetComponent<Gun2>();
        gun.localPosition = Vector3.zero;
        gun.localRotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    public void ResetLevel()
    {
        Time.timeScale = 1;
        slow = false;
        m_Enemies.Clear();
    }

    public async UniTask ActivateFloor(Floor _floor)
    {
        ResetLevel();
        _floor.ActivateFloor();
        await UniTask.Delay(1000);
        m_Enemies = _floor.m_Enemies;
    }

    private void Update()
    {
        if (m_Enemies.Count > 0)
        {
            if (m_Enemies.Any(IsSlow))
            {
                // Physics.autoSimulation = false;
                Physics.Simulate(Time.fixedDeltaTime * 0.25f);
                EventManager1<bool>.CallEvent(GameEvent.SLOWMOTION, true);
            }
            else
            {
                // Physics.autoSimulation = true;
                Physics.Simulate(Time.fixedDeltaTime);
                EventManager1<bool>.CallEvent(GameEvent.SLOWMOTION, false);
                // Physics.Simulate(0.25f * 0.02);
            }
            // Time.timeScale = m_Enemies.Any(IsSlow) ? Time.timeScale = 0.25f : Time.timeScale = 1f;
            // if (!IsZooming)
            // {
            //     if (m_Enemies.Any(IsSlow))
            //     {
            //         if (m_CMCam.m_Lens.FieldOfView != 50f)
            //         {
            //             CameraZoomInAnimation(50f, 0.4f);   
            //         }
            //     }
            //     else
            //     {
            //         if (m_CMCam.m_Lens.FieldOfView != 60f)
            //         {
            //             CameraZoomOutAnimation(50, 0.4f);
            //         }
            //     }
            // }
        }


        // RaycastHit hitInfo;

        Shooter2 shooter = LevelController2.Instance.m_Shooter;

        if (Input.GetMouseButtonDown(0) && !shooter.IsState(P_DeathState2.Instance) && GameManager.Instance.m_GameLoop == GameLoop.Play)
        {

            RaycastHit hitInfo;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // List<RaycastHit> rayHits = Physics.SphereCastAll(ray, 0.25f, Mathf.Infinity, m_IgnoreLayer, QueryTriggerInteraction.Ignore).ToList();
            List<RaycastHit> rayHits = Physics.SphereCastAll(ray, 0.25f, Mathf.Infinity, m_IgnoreLayer).ToList();

            if (rayHits.Any(x => x.transform.GetComponent<IEnemy2>() != null))
            {
                rayHits.RemoveAll(x =>
                    (x.transform.GetComponent<IEnemy2>() == null));
            }

            // if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, m_IgnoreLayer))
            if (rayHits.Count > 0)
            {

                hitInfo = rayHits.OrderBy(x => (x.point - shooter.transform.position).sqrMagnitude).FirstOrDefault();
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
                    else if (go.tag.Equals("Ground") || go.tag.Equals("Gas") || go.tag.Equals("Untagged") || go.tag.Equals("Furniture"))
                    {
                        Transform trans = hitInfo.collider.GetComponent<Transform>();
                        // tf_LookAimIK.position = hitInfo.point;
                        Shoot(hitInfo.point, trans);
                    }
                }
            }
        }
    }

    public async UniTask CameraDeathAnimation(Vector3 targetPosition, float duration)
    {
        tf_HeliHolder.parent = null;

        float time = 0;
        Vector3 startPosition = m_CMCamOffset.m_Offset;
        while (time < duration)
        {
            m_CMCamOffset.m_Offset = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        m_CMCamOffset.m_Offset = targetPosition;
        PopupCaller.OpenPopup(UIID.POPUP_LOSE);
    }

    public async UniTask CameraZoomInAnimation(float fov, float duration)
    {
        IsZooming = true;

        float time = 0f;
        while (time < duration)
        {
            m_CMCam.m_Lens.FieldOfView = Mathf.Lerp(60f, fov, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        m_CMCam.m_Lens.FieldOfView = fov;
        IsZooming = false;
    }

    public async UniTask CameraZoomOutAnimation(float fov, float duration)
    {
        IsZooming = true;

        float time = 0f;
        while (time < duration)
        {
            m_CMCam.m_Lens.FieldOfView = Mathf.Lerp(50f, fov, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        m_CMCam.m_Lens.FieldOfView = fov;
        IsZooming = false;
    }

    public async UniTask CameraOffset(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = m_CMCamOffset.m_Offset;
        while (time < duration)
        {
            m_CMCamOffset.m_Offset = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        m_CMCamOffset.m_Offset = targetPosition;
    }

    public void Shoot(Vector3 _lookAt, Transform _tfEnemy)
    {
        EventManager1<bool>.CallEvent(GameEvent.MODE_2_SHOOTER_SHOT, true);
        GameObject go = PrefabManager.Instance.SpawnVFXPool("VFX_2", m_GunIngame.tf_FirePoint.position);
        go.transform.LookAt(_lookAt);
        go.transform.parent = m_GunIngame.tf_FirePoint;
        PrefabManager.Instance.SpawnBulletPool("Bullet1", m_GunIngame.tf_FirePoint.position).GetComponent<Bullet>().Fire(_lookAt, _tfEnemy);
    }

    public bool IsSlow(Enemy2 _enemy)
    {
        Vector3 targetDir = transform.position - _enemy.tf_Owner.position;
        targetDir = targetDir.normalized;

        float dot = Vector3.Dot(targetDir, _enemy.tf_Owner.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // if (angle < _enemy.m_DetectDegree && !_enemy.IsState(AimState.Instance))
        if (angle < _enemy.m_DetectDegree)
        {
            // if (_enemy.isActive && !_enemy.IsState(AimState.Instance))
            if (_enemy.isActive)
            {
                _enemy.SetInRange(true);
                return true;
            }

            return false;
        }

        _enemy.SetInRange(false);
        return false;
    }
}
