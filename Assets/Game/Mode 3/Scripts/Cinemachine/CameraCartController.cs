using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;

public class CameraCartController : MonoBehaviour 
{
    [SerializeField] private bool followTarget;
    public CinemachineDollyCart cart;
    public CinemachineVirtualCamera virtualCamera;

    // private void Awake()
    // {
    //     cart = GetComponent<CinemachineDollyCart>();
    //     virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    // }

    // public async UniTask InitDolly(CinemachineSmoothPath dollyTrack, Transform target, float speed = 0f)
    public void InitDolly(CinemachineSmoothPath dollyTrack, Transform target, float speed = 0f)
    {
        if(speed != 0f)
            cart.m_Speed = speed;

        // await UniTask.WaitUntil(() => dollyTrack.isActiveAndEnabled == true);
        cart.m_Path = dollyTrack;
        
        if(followTarget)
            virtualCamera.m_LookAt = target;
        
    }
}
