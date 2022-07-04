using System;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2;
using Cysharp.Threading.Tasks;
using Exploder.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public class PlayerShootingController : Singleton<PlayerShootingController>
{
    public BulletTimeController bulletTimeController;
    public Transform bulletSpawnTransform;
    public Scope scope;
    public Camera m_CameraAim;
    public float shootingForce;
    public float minDistanceToPlayAnimation;
    public bool isScopeEnabled = false;
    public bool isShooting = false;
    private float scrollInput = 0f;
    private bool wasScopeOn;

    public TouchTrackPad m_TrackPad;

    [Title("Scope Rotation")]
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";
    private float horizontalInput;
    private float verticalInput;
    private float mouseInputX;
    private float mouseInputY;
    private float currentRotationY;
    private float currentRotationX;
    private float mouseSensivity;
    public float minMouseSensivity;
    public float maxMouseSensivity;
    public float rotationSpeed;
    public Transform tf_GunHolder;

    [Title("Gun")]
    public Gun3 m_GunIngame;
    public GunInventoryConfig m_GunInventoryConfig;

    private void Start()
    {
        mouseSensivity = maxMouseSensivity;
        currentRotationY = transform.eulerAngles.y;
        currentRotationX = transform.eulerAngles.x;
    }

    private void OnEnable()
    {
        int curGunMode2 = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode3);
        var gunInvent = m_GunInventoryConfig.m_GunItem.Find(x => x.m_ID == curGunMode2);

        SpawnGun(gunInvent);
    }

    private void Update()
    {
        GetInput();
        // GetInputRotation();
        // HandleRotation();
    }

    public void SpawnGun(GunInventoryItem _gunInvent)
    {
        if (m_GunIngame != null)
        {
            PrefabManager.Instance.DespawnPool(m_GunIngame.gameObject);
            m_GunIngame = null;
        }

        Transform gun = GameObject.Instantiate(_gunInvent.go_UIPrefabInGame, tf_GunHolder).GetComponent<Transform>();
        m_GunIngame = gun.GetComponent<Gun3>();

        bulletSpawnTransform = m_GunIngame.bulletSpawnTransform;
        scope = m_GunIngame.scope;
        m_CameraAim = m_GunIngame.m_CameraAim;

        gun.localPosition = Vector3.zero;
        gun.localRotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    private async UniTask Shoot()
    {
        if (Physics.Raycast(m_CameraAim.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit))
        {
            IBodyPart IBodyPart = hit.collider.GetComponent<IBodyPart>();
            Vector3 direction = hit.point - bulletSpawnTransform.position;
            if (IBodyPart != null)
            {
                if (IBodyPart.OnCanSlowmotion()) //LOGIC TRIGGER BULLET TIME
                {
                    GameObject go = PrefabManager.Instance.SpawnBulletPool("Bullet", bulletSpawnTransform.position, bulletSpawnTransform.rotation);
                    Bullet3 bullet3Instance = go.GetComponent<Bullet3>();
                    await UniTask.WaitUntil(() => bullet3Instance.isActiveAndEnabled == true);
                    bullet3Instance.Launch(shootingForce, hit.collider.transform, hit.point, hit);
                    bulletTimeController.StartSequence(bullet3Instance, hit.point);
                }
                else
                {
                    IBodyPart.OnHit();
                }
            }
        }
    }

    private void GetInputRotation()
    {
        horizontalInput = Input.GetAxisRaw(HORIZONTAL);
        verticalInput = Input.GetAxisRaw(VERTICAL);

        Vector2 mouseInput = new Vector2(CF2Input.GetAxis("Mouse X"), CF2Input.GetAxis("Mouse Y")) * 1f;
        mouseInputX = mouseInput.x;
        mouseInputY = mouseInput.y;

        // mouseInputX = Input.GetAxis(MOUSE_X);
        // mouseInputY = Input.GetAxis(MOUSE_Y);

        mouseSensivity = minMouseSensivity + scope.GetZoomPrc() * Mathf.Abs(minMouseSensivity - maxMouseSensivity);
    }

    private void HandleRotation()
    {
        float yaw = mouseInputX * Time.deltaTime * rotationSpeed * mouseSensivity;
        currentRotationY += yaw;
        float pitch = mouseInputY * Time.deltaTime * rotationSpeed * mouseSensivity;
        currentRotationX -= pitch;
        currentRotationX = Mathf.Clamp(currentRotationX, -90, 90);
        tf_GunHolder.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
        transform.localRotation = Quaternion.Euler(0, currentRotationY, 0);
    }

    private void GetInput()
    {
        if (m_TrackPad.JustPressed())
        {
            scope.ChangeScopeFOV();
            scope.SetScopeFlag(true);
        }

        if (m_TrackPad.Pressed())
        {
            GetInputRotation();
            HandleRotation();
        }

        if (m_TrackPad.JustReleased())
        {
            scope.SetScopeFlag(false);
            Shoot();
        }
    }
}
