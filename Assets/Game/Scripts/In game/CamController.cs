using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SRF.UI.Layout;
using Unity.VisualScripting;
using UnityEngine;
using ControlFreak2;

public class CamController : Singleton<CamController>
{
    public Transform tf_Owner;
    public Transform tf_FirePoint;
    public Transform tf_LookAimIK;
    public LayerMask ignore;
    public CinemachineVirtualCamera m_CMCam, m_CMCam_Death;
    public CinemachineCameraOffset m_CMCamOffset;

    public Transform tf_MainCamHolder;
    public Transform tf_HeliHolder;
    public Transform tf_HeliHolderPoint;
    public Transform tf_ShooterHolder;

    public RectTransform mainCanvas;

    public float m_ShootTime = 0.4f;

    public int m_IgnoreLayer = ~(1 << 0 | 1 << 9 | 1 << 12 | 1 << 14);

    public TouchTrackPad m_TrackPad;
    
    void Update()
    {
        m_ShootTime += Time.deltaTime;
        if (m_TrackPad.Pressed())
        {
            Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 0.35f;
            RectTransform tfCrosshair = InGameManager.Instance.img_Crosshair.GetComponent<RectTransform>();
            Vector2 apos = tfCrosshair.anchoredPosition;
            float xPos = apos.x;
            float yPos = apos.y;

            xPos = Mathf.Clamp(xPos, 0f, mainCanvas.rect.width);
            yPos = Mathf.Clamp(yPos, 0f, mainCanvas.rect.height);

            tfCrosshair.anchoredPosition = new Vector2(xPos + mouseInput.x * 50f, yPos + mouseInput.y * 50f);

            var ray = Camera.main.ScreenPointToRay(tfCrosshair.position);
            
            RaycastHit hitInfo;
            // if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, m_IgnoreLayer))
            if (Physics.SphereCast(ray, 0.3f, out hitInfo, Mathf.Infinity, m_IgnoreLayer))
            // if (Physics.SphereCastAll(ray, 1f, out hitInfo, Mathf.Infinity, m_IgnoreLayer))
            // if (Physics.SphereCastAll(ray, out hitInfo, Mathf.Infinity, m_IgnoreLayer))
            {
                tf_LookAimIK.position = hitInfo.point;
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
            tf_ShooterHolder.parent = tf_MainCamHolder;
            tf_ShooterHolder.localPosition = new Vector3(-0.213f, -0.838f, 0.54f);
            tf_ShooterHolder.localRotation = Quaternion.Euler(0f, 0f, 0f);
            Debug.Log("BBBBBBBBBBBB");
        }

        await UniTask.WaitUntil(() => LevelController.Instance != null);

        m_CMCam.Follow = LevelController.Instance.tf_CamLookPoint;
        m_CMCam.LookAt = LevelController.Instance.tf_CamLookPoint;

        m_CMCamOffset.m_Offset = new Vector3(0f, 5f, 0f);
        
        await UniTask.Delay(100);
    }

    public void Shoot(Vector3 _pos, Transform _tfEnemy)
    {
        GameObject go = PrefabManager.Instance.SpawnVFXPool("VFX_2", tf_FirePoint.position);
        go.transform.LookAt(_pos);
        go.transform.parent = tf_LookAimIK;
        PrefabManager.Instance.SpawnBulletPool("Bullet1", tf_FirePoint.position).GetComponent<Bullet>().Fire(_pos, _tfEnemy);
    }

    public async UniTask CameraDeath(Vector3 targetPosition, float duration)
    {
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

        // await UniTask.Delay(2000);
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
            
        tf_ShooterHolder.parent = tf_MainCamHolder;
        tf_ShooterHolder.localPosition = new Vector3(-0.213f, -0.838f, 0.54f);
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
