using System;
using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Text;
using Dreamteck.Splines;
using UnityEngine;

public class TestSpline : MonoBehaviour
{
    public List<SplineComputer> m_SplineComputers;
    public SplineFollower m_Follower;
    public bool isFollowing;
    public int m_PathNo;

    private void OnEnable()
    {
        m_Follower.onBeginningReached += StartFollwing;
        m_Follower.onEndReached += StopFollwing;
    }

    private void OnDisable()
    {
        m_Follower.onBeginningReached -= StartFollwing;
        m_Follower.onEndReached -= StopFollwing;
    }

    private void Start()
    {
        m_PathNo = 0;
        // if (m_PathNo > 0) 
        //     m_SplineComputers[m_PathNo].gameObject.SetActive(true);
        //     m_SplineComputers[m_PathNo - 1].gameObject.SetActive(false);
        
        // m_SplineComputers[m_PathNo].gameObject.SetActive(true);
        m_Follower.spline = m_SplineComputers[m_PathNo];
            // m_Follower.autoStartPosition = true;
    }

    private void Update()
    {
        if (Helper.GetKeyDown(KeyCode.A))
        {
            if (!isFollowing)
            {
                if (m_PathNo < m_SplineComputers.Count - 1) m_PathNo++;
                else m_PathNo = 0;

                // if (m_PathNo > 0)
                // {
                //     // m_SplineComputers[m_PathNo].gameObject.SetActive(true);
                //     // m_SplineComputers[m_PathNo - 1].gameObject.SetActive(false);
                //     m_Follower.spline = m_SplineComputers[m_PathNo];
                // }
                // else
                // {
                //     // m_SplineComputers[m_PathNo].gameObject.SetActive(true);
                //     // m_SplineComputers[m_SplineComputers.Count - 1].gameObject.SetActive(false);
                //     m_Follower.spline = m_SplineComputers[m_PathNo];
                // }     
                
                m_Follower.spline = m_SplineComputers[m_PathNo];
                
                m_Follower.SetPercent(0);
            }
        }
    }

    public void StartFollwing(double _aaa)
    {
        Debug.Log("AAAAAAAAAA");
        isFollowing = true;
    }
    
    public void StopFollwing(double _bbb)
    {
        Debug.Log("BBBBBBBBBBB");
        isFollowing = false;
    }
}
