using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PlayerMovevement : MonoBehaviour
{
    public CharacterController m_CC;
    public Rigidbody m_Rb;
    public Waypoint m_CurWaypoint;
    public int m_WaypointNo;
    public float m_Speed;

    public Transform tf_right, tf_Left;
    
    public Quaternion forwardDirection = Quaternion.identity;
    public Quaternion rightDirection = Quaternion.FromToRotation(Vector3.forward, Vector3.right);
    public Quaternion leftDirection = Quaternion.FromToRotation(Vector3.forward, Vector3.left);
    public Quaternion backDirection = Quaternion.FromToRotation(Vector3.forward, Vector3.back);

    private void Start()
    {
        m_WaypointNo = 0;
        m_CurWaypoint = WaypointController.Instance.m_Waypoints[m_WaypointNo];
        Vector3 look = new Vector3(0f, m_CurWaypoint.transform.position.y, 0f);
        Quaternion quar = new Quaternion(0f, look.y, 0f, 0f);

        Vector3 origin = new Vector3(0f, m_CurWaypoint.transform.rotation.y, 0f);
        Vector3 des = transform.forward;

        Vector3 look2 = des - origin;
        
        // transform.DORotateQuaternion(quar, 1f);
        // transform.DODynamicLookAt()
        // transform.DOLookAt(m_CurWaypoint.transform.position, 0f, AxisConstraint.Y, Vector3.up);
        // m_Rb.DORotate(look2, 0.5f);
    }

    private void FixedUpdate()
    {
        if (Helper.GetKey(KeyCode.A))
        {
            // m_CC.Move(transform.forward * Time.deltaTime);
            // m_Rb.MovePosition(m_Rb.position + (m_Rb.transform.forward.normalized * m_Speed * Time.fixedDeltaTime));
            // m_Rb.AddForce(m_Rb.transform.forward.normalized * m_Speed);
        }

        // m_Rb.MovePosition(m_Rb.position + (m_Rb.transform.forward.normalized * m_Speed * Time.fixedDeltaTime));
        
        // Debug.DrawRay(m_Rb.transform.position, m_Rb.transform.position.normalized * 5f, Color.green);
        Debug.DrawRay(m_Rb.transform.position, m_Rb.transform.forward.normalized * 5f, Color.green);
        Debug.DrawRay(m_Rb.transform.position, m_Rb.transform.right.normalized * 5f, Color.red);
        
        if (Helper.GetKeyDown(KeyCode.D))
        {
            // m_CurWaypoint = WaypointController.Instance.m_Waypoints[m_WaypointNo];
            // Vector3 look = new Vector3(0f, m_CurWaypoint.transform.position.y, 0f);
            // Quaternion quar = new Quaternion(0f, look.y, 0f, 0f);
            // transform.DOLocalRotateQuaternion(quar, 0.75f);
            LookWaypoint(Vector3.back, 2f, Direction.RIGHT);
            // m_Rb.DOKill(true);
            // m_Rb.DORotate(m_Rb.transform.forward + Vector3.left, 1f);
            // m_Rb.transform.rotation = Quaternion.FromToRotation(m_Rb.transform.forward, m_Rb.transform.right);
            // Debug.Log("Degree: " + m_Rb.transform.forward);
            // m_Rb.rotation = Quaternion.LookRotation(m_Rb.transform.forward.normalized - m_Rb.transform.right.normalized,  m_Rb.transform.up);
        }
    }
    
    public async UniTask LookWaypoint(Vector3 targetPosition, float duration, Direction _dir)
    {
        float time = 0;
        Quaternion lookOnLook = new Quaternion(0f, 0f, 0f, 0f);

        if (_dir == Direction.LEFT)
        {
            // lookOnLook = Quaternion.LookRotation(m_Rb.transform.position - transform.right * 3f);
            // lookOnLook = Quaternion.LookRotation(m_Rb.transform.position - transform.right * 3f);
            Debug.Log("Look left");
        }
        else if(_dir == Direction.RIGHT)
        {
            // lookOnLook = Quaternion.SetLookRotation(m_Rb.transform.right - m_Rb.transform.forward);
            // lookOnLook = m_Rb.transform.right.normalized - m_Rb.transform.forward.normalized;
            Debug.Log("Look right");
        }

        // Quaternion look = lookOnLook;

        Vector3 a = new Vector3(0f, 90f, 0f);
        Vector3 look = transform.rotation.eulerAngles + a;
        
        while (time < duration)
        {
            m_Rb.rotation = Quaternion.Slerp(m_Rb.transform.rotation, lookOnLook, time / duration);
            time += Time.fixedDeltaTime;
            await UniTask.Yield();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Calllllllllllllllll");
        if (other.tag.Equals("Waypoint"))
        {
            LookWaypoint(Vector3.one, 2f, m_CurWaypoint.m_Dir);
            
            m_WaypointNo++;
            m_CurWaypoint = WaypointController.Instance.m_Waypoints[m_WaypointNo];
            Vector3 targetPosition = new Vector3(0f, m_CurWaypoint.transform.position.y, 0f);
            
        }
    }
}
