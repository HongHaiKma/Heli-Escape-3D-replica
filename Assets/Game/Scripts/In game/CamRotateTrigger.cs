using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RootMotion;
using UnityEngine;

public class CamRotateTrigger : MonoBehaviour
{
    public float m_YAxis;
    public bool m_CamOffSetChange;
    public float m_CamOffsetYAxis;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Hostage"))
        {
            if (m_CamOffSetChange)
            {
                CamController.Instance.CameraOffset(new Vector3(0f, m_CamOffsetYAxis, 0f), 2f);
            }
            
            LevelController.Instance.m_Hostage.tf_LookAtPoint.DOLocalRotateQuaternion(Quaternion.Euler(0f, m_YAxis, 0f), 1.5f).OnComplete(
                () =>
                {
                    LevelController.Instance.m_Hostage.tf_LookAtPoint.DOKill();
                    Vector3 origin = new Vector3(0f, (LevelController.Instance.m_Hostage.tf_LookAtPoint.position.y - 60f) + 100f, 0f);
                    Vector3 end = new Vector3(0f, (LevelController.Instance.m_Hostage.tf_LookAtPoint.position.y + 60f) + 100f, 0f);
                    LevelController.Instance.m_Hostage.tf_LookAtPoint.DOLocalRotate(end, 5f).SetLoops(-1, LoopType.Yoyo).startValue = origin;
                });
        }
    }
}
