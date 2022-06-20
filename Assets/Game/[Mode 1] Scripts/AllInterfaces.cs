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

interface IDamageable3
{
    void OnHit(Vector3 shootDirection, Rigidbody shotRB, bool isDie);
}

interface IBodyPart
{
    void OnHit();
}