using UnityEngine;

public class BallTrapSystem : TrapSystem
{
    public Joint m_Joint;
    public Rigidbody rb_Owner;

    public override void ActivateTrap()
    {
        base.ActivateTrap();
        rb_Owner.constraints = RigidbodyConstraints.None;
        m_Joint.connectedBody = null;
    }
}
