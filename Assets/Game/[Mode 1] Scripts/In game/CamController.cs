using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SRF.UI.Layout;
using Unity.VisualScripting;
using UnityEngine;
using ControlFreak2;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class CamController : Singleton<CamController>
{
    public Transform tf_Owner;
    public Transform tf_FirePoint;
    public Transform tf_LookAimIK;
    public CinemachineVirtualCamera m_CMCam;
    public CinemachineCameraOffset m_CMCamOffset;

    public Transform tf_MainCamHolder;
    public Transform tf_HeliHolder;
    public Transform tf_HeliHolderPoint;
    public Transform tf_ShooterHolder;
    

    public RectTransform mainCanvas;

    public float m_ShootTime = 0.4f;

    // public int m_IgnoreLayer = ~(1 << 0 | 1 << 9 | 1 << 12 | 1 << 14);
    public int m_IgnoreLayer = ~(1 << 0 | 1 << 9 | 1 << 12 | 1 << 14);

    public TouchTrackPad m_TrackPad;


    private RaycastHit hitInfo;
    
    [Title("Gun Data")]
    public Transform tf_GunHolder;
    public GunInventoryConfig m_GunInventoryConfig;

    private async UniTask OnEnable()
    {
        await UniTask.WaitUntil(() => LevelController.Instance != null);
        
        int curGunMode1 = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode1);
        var gunInvent = m_GunInventoryConfig.m_GunItem.Find(x => x.m_ID == curGunMode1);

        if (gunInvent != null)
            Transform gun = GameObject.Instantiate(gunInvent.go_UIPrefabInGame).GetComponent<Transform>();
        
        m_CMCam.Follow = LevelController.Instance.tf_CamIntroFollower;
        m_CMCam.LookAt = LevelController.Instance.tf_CamIntroFollower;
        CameraOffset(new Vector3(30f, 30f, 30f), 0f);
    }

    void Update()
    {
        m_ShootTime += Time.deltaTime;
        if (m_TrackPad.Pressed())
        {
            Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.5f;
            RectTransform tfCrosshair = UIIngame.Instance.img_Crosshair.GetComponent<RectTransform>();
            Vector2 apos = tfCrosshair.anchoredPosition;
            float xPos = apos.x;
            float yPos = apos.y;

            xPos = Mathf.Clamp(xPos, 0f, mainCanvas.rect.width);
            yPos = Mathf.Clamp(yPos, 0f, mainCanvas.rect.height);

            tfCrosshair.anchoredPosition = new Vector2(xPos + mouseInput.x * 50f, yPos + mouseInput.y * 50f);
            
            
            var ray = Camera.main.ScreenPointToRay(tfCrosshair.position);
            
            List<RaycastHit> rayHits = Physics.SphereCastAll(ray, 0.25f, Mathf.Infinity, m_IgnoreLayer, QueryTriggerInteraction.Ignore).ToList();

            if (rayHits.Count > 0)
            {
                hitInfo = rayHits.OrderBy(x => (x.point - tf_Owner.position).sqrMagnitude).FirstOrDefault();
                // tf_LookAimIK.position = hitInfo.point;
                
                tf_GunHolder.rotation = Quaternion.LookRotation(hitInfo.point - tf_GunHolder.position);
            }

            if (m_ShootTime <= 0.1f)
            {
                return;
            }


            if (rayHits.Any(x => x.transform.GetComponent<IDamageable>() != null))
            {
                rayHits.RemoveAll(x =>
                    (x.transform.GetComponent<IDamageable>() == null && x.transform.GetComponent<ITrap>() == null));
                
                rayHits.RemoveAll(x =>
                    (x.transform.GetComponent<Enemy>() != null && x.transform.GetComponent<Enemy>().IsState(DeathState.Instance)));
            }

            // if (Physics.SphereCast(ray, 0.3f, out hitInfo, Mathf.Infinity, m_IgnoreLayer, QueryTriggerInteraction.Ignore))
            // if (Physics.SphereCast(ray, 0.3f, out hitInfo, Mathf.Infinity, m_IgnoreLayer, QueryTriggerInteraction.Ignore))
            // if (Physics.SphereCastAll(ray, 0.5f, out hitInfo, Mathf.Infinity, m_IgnoreLayer, QueryTriggerInteraction.Ignore))
            if(rayHits.Count > 0)
            {
                // tf_GunHolder.LookAt(hitInfo.point);
                if (m_ShootTime > 0.1f)
                {
                    Collider col = hitInfo.collider;
                    GameObject go = hitInfo.transform.gameObject;
            
                    if (col != null)
                    {
                        m_ShootTime = 0f;
            
                        if (col.tag.Equals("Ground"))
                        {
                            PrefabManager.Instance.SpawnVFXPool("BulletHole", hitInfo.point);
                            PrefabManager.Instance.SpawnVFXPool("BulletImpact", hitInfo.point);
                            PrefabManager.Instance.SpawnVFXPool("VFX_2", tf_FirePoint.position);
                        }
            
                        if (col.gameObject.GetComponent<IDamageable>() != null)
                        {
                            col.gameObject.GetComponent<IDamageable>().OnHit(hitInfo.point);
                            PrefabManager.Instance.SpawnVFXPool("VFX_2", tf_FirePoint.position);
                        }
            
                        if (col.gameObject.GetComponent<ITrap>() != null)
                        {
                            col.gameObject.GetComponent<ITrap>().OnTrigger();
                        }
                    }
                }
            }
        }
    }

    public async UniTask ResetLevel()
    {
        if (tf_HeliHolder.parent == null)
        {
            tf_HeliHolder.parent = tf_MainCamHolder;
            tf_HeliHolder.localPosition = new Vector3(0.2f, -1.6f, 1f);
            tf_HeliHolder.localRotation = Quaternion.Euler(0f, 0f, 0f);
            Debug.Log("AAAAAAAAAAAA");
        }
            
        if (tf_ShooterHolder.parent == null)
        {
            tf_ShooterHolder.parent = tf_HeliHolder;
            tf_ShooterHolder.localPosition = new Vector3(-0.4129976f, 0.762f, -0.4599994f);
            tf_ShooterHolder.localRotation = Quaternion.Euler(0f, 0f, 0f);
            Debug.Log("BBBBBBBBBBBB");
        }

        await UniTask.WaitUntil(() => LevelController.Instance != null);

        m_CMCam.Follow = LevelController.Instance.tf_CamLookPoint;
        m_CMCam.LookAt = LevelController.Instance.tf_CamLookPoint;

        // m_CMCamOffset.m_Offset = new Vector3(5f, 5f, 5f);
        
        await UniTask.Delay(100);
    }

    public void PlayGame()
    {
        m_CMCam.Follow = LevelController.Instance.tf_CamIntroFollower;
        m_CMCam.LookAt = LevelController.Instance.tf_CamIntroFollower;
        CameraIntro(new Vector3(15f, 15f, -4.5f), 2.5f);
    }

    public async UniTask CameraIntro(Vector3 targetPosition, float duration)
    {
        await UniTask.WhenAll(ResetLevel());
        m_CMCamOffset.enabled = true;
        m_CMCamOffset.m_Offset = new Vector3(-10f, 13f, -8f);
        
        tf_HeliHolder.parent = null;
        tf_ShooterHolder.parent = null;
        
        float time = 0;
        Vector3 startPosition = m_CMCamOffset.m_Offset;
        while (time < duration)
        {
            m_CMCamOffset.m_Offset = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        m_CMCamOffset.m_Offset = targetPosition;
        
        tf_HeliHolder.parent = tf_MainCamHolder;
        tf_HeliHolder.localPosition = new Vector3(0.2f, -1.6f, 1f);
        tf_HeliHolder.localRotation = Quaternion.Euler(0f, 0f, 0f);
            
        tf_ShooterHolder.parent = tf_HeliHolder;
        tf_ShooterHolder.localPosition = new Vector3(-0.4129976f, 0.762f, -0.4599994f);
        tf_ShooterHolder.localRotation = Quaternion.Euler(0f, 0f, 0f);
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

    public async UniTask CamMoveEndPos(float duration)
    {
        m_CMCamOffset.enabled = false;
        m_CMCam.Follow = null;
        m_CMCam.LookAt = LevelController.Instance.tf_CamLook;
        
        float time = 0;
        Vector3 startPosition = tf_Owner.position;
        Vector3 targetPosition = LevelController.Instance.tf_CamPos.position;
        
        while (time < duration)
        {
            tf_Owner.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        tf_Owner.position = targetPosition;
    }

    public async UniTask CamMoveFinish(float duration)
    {
        m_CMCamOffset.enabled = false;
        tf_HeliHolder.parent = null;
        tf_ShooterHolder.parent = null;
        m_CMCam.Follow = null;
        m_CMCam.LookAt = LevelController.Instance.tf_CamPos;
        
        float time = 0;
        Vector3 startPosition = tf_Owner.position;
        Vector3 targetPosition = LevelController.Instance.tf_CamFinish.position;
        
        while (time < duration)
        {
            tf_Owner.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            await UniTask.Yield();
        }

        tf_Owner.position = targetPosition;
    }
}
