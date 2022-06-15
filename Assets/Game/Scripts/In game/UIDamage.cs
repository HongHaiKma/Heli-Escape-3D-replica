using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIDamage : VFXEffect
{
    public override void OnEnable()
    {
        m_LifeTime = 0;
        // tf_Owner.SetParent(UIIngame.Instance.tf_MainCanvas);
        StartCoroutine(IEUpdate());
    }

    public void Fly(Vector3 _target, Transform _parent)
    {
        tf_Owner.SetParent(_parent);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_target);
        tf_Owner.position = screenPos;
        tf_Owner.DOMove(tf_Owner.position + new Vector3(0f, 100f, 0f), 0.5f);
    }
}
