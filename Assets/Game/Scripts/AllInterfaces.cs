using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnHit(Vector3 _pos);
}

interface ITrap
{
    void OnTrigger();
}

interface ITriggerble
{
    void OnTrigger();
}