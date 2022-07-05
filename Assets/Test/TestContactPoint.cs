using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Micosmo.SensorToolkit;

public class TestContactPoint : MonoBehaviour
{
    public Transform tf_Wall;
    public Transform tf_HitPoint;
    // void Update()
    // {
    //     transform.position += transform.forward.normalized * 2 * Time.deltaTime;
    // }

    // void OnTriggerEnter(Collider other)
    // {

    // }

    // tf_GunHolder.rotation = Quaternion.LookRotation(hitInfo.point - tf_GunHolder.position);

    // private void Update()
    // {
    //     Physics.Linecast(transform.position, tf_Wall.position))
    // }

    void OnCollisionEnter(Collision collision)
    {
        // List<ContactPoint>
        // Vector3 look = collision.contacts.OrderBy(x => (x.point - this.transform.position).sqrMagnitude).First().point;
        Vector3 aaa = tf_Wall.position - transform.position;
        // transform.rotation = Quaternion.LookRotation(look - transform.position);
        // transform.LookAt(new Vector3(0f, look.y, 0f));
    }

    [Button]
    public void Test()
    {
        // Vector3 aaa = tf_Wall.position - transform.position;
        // RaycastHit hit;
        // Vector3 opposite = Vector3.zero;
        // // hitt = Physics.Linecast(transform.position, tf_Wall.position);
        // if (Physics.Linecast(transform.position, tf_Wall.position, out hit))
        // {
        //     Helper.DebugLog("Position: " + hit.point);
        //     opposite = -hit.normal;
        // }

        // transform.rotation = Quaternion.LookRotation(new Vector3(opposite.normalized.x, 0f, opposite.normalized.z), Vector3.up);

        // RayHit hit = m_RaySensor.GetObstructionRayHit();
        // Helper.DebugLog("Point: " + hit.Normal);
        // tf_HitPoint.position = hit.Point;

        // Physics.OverlapSphere(transform.position, 1f, );
    }

    void OnDrawGizmos()
    {
        // Debug.DrawLine(transform.position, tf_Wall.position, Color.red);
        // Physics.Linecast(transform.position, tf_Wall.position);
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(transform.position, 1f);

        // Debug.Draw
    }
}
