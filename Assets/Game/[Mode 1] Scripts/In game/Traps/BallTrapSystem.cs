using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrapSystem : TrapSystem
{
    public Joint m_Joint;

    public override void ActivateTrap()
    {
        base.ActivateTrap();
        m_Joint.connectedBody = null;
    }
}
