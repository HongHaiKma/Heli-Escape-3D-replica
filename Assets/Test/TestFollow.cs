using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Exploder.Utils;
using Pathfinding;
using Sirenix.Utilities;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

public class TestFollow : Singleton<TestFollow>
{
    // public bool a;
    // public bool b;
    //
    // public AIPath ai;
    // public Transform tf_Target;
    //
    // private void Update()
    // {
    //     ai.destination = tf_Target.position;
    // }
    
    [SerializeField]
    public List<TestCallFunction> a;

    private void Update()
    {
        if (Helper.GetKeyDown(KeyCode.A))
        {
            // TestTime();
            // TestTime2();
            // Test3();
            Measure(Test3, "INumerable");
            Measure(Test4, "List");
            // Measure(Test5, "List Linq");
        }
    }

    public void TestTime()
    {
        var timer = new Stopwatch();
        timer.Start();
        
        // for (int i = 0; i < a.Count; i++)
        // {
        //     Helper.DebugLog("AAAAAAAAAA");
        // }

        // List<TestCallFunction> b = a;

        for (int i = 0; i < a.Count; i++)
        {
            a[i].TestFunc();
        }

        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;
        Helper.DebugLog("Time: " + timeTaken.TotalMilliseconds);
    }
    
    public void TestTime2()
    {
        var timer = new Stopwatch();
        timer.Start();
        
        // for (int i = 0; i < a.Count; i++)
        // {
        //     Helper.DebugLog("AAAAAAAAAA");
        // }

        // IEnumerable<TestCallFunction> b = a;
        //
        // for (int i = 0; i < b.Count; i++)
        // {
        //     b[i].TestFunc();
        // }
        
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;
        Helper.DebugLog("Time: " + timeTaken.TotalMilliseconds);
    }

    public void Test3()
    {
        Helper.DebugLog("Test 3");
        
        List<TestCallFunction> aaa = AllEnemiesLowHealth(a).ToList();

        for (int i = 0; i < aaa.Count; i++)
        {
            Helper.DebugLog("Name: " + aaa[i].name);
        }
    }
    
    public List<string> someList;

    public void Test4()
    {
        Helper.DebugLog("Test 4");
        // for (int i = 0; i < a.Count; i++)
        // {
        //     if (a[i].m_Health < 3f)
        //     {
        //         Helper.DebugLog("Name: " + a[i].name);  
        //     }
        // }
        
        List<TestCallFunction> aaa = AllEnemiesLowHealth2(a);

        for (int i = 0; i < aaa.Count; i++)
        {
            Helper.DebugLog("Name: " + aaa[i].name);
        }

        
        // IEnumerable myEnumerable = someList.Where(x=>x != "not me").ToList();
        //
        // Helper.DebugLog("Name: " + myEnumerable[3]);
    }
    
    public async UniTask Test5()
    {
        Helper.DebugLog("Test 4");
        // for (int i = 0; i < a.Count; i++)
        // {
        //     if (a[i].m_Health < 3f)
        //     {
        //         Helper.DebugLog("Name: " + a[i].name);  
        //     }
        // }
        
        var aaa = AllEnemiesLowHealth3(a);

        for (int i = 0; i < aaa.Count; i++)
        {
            Helper.DebugLog("Name: " + aaa[i].name);
        }

        await UniTask.CompletedTask;
        // IEnumerable myEnumerable = someList.Where(x=>x != "not me").ToList();
        //
        // Helper.DebugLog("Name: " + myEnumerable[3]);
    }

    // GraphNode nearestNode = AstarPath.active.GetNearest(tf_Owner.position, NNConstraint.Default).node;
    // if (nearestNode != null)
    // {
    //   if (nearestNode.Walkable)
    //           {
    //               Int3 a = new Int3();
    //               Vector3 b = (Vector3)a;
    //                              
    //               // m_AIPath.destination = b;
    //               rb_Owner.position = (Vector3)nearestNode.position;
    //           }  
    // }
    
    public IEnumerable<TestCallFunction> AllEnemiesLowHealth(IEnumerable<TestCallFunction> sample)
    {
        return from a in sample
            where a.m_Health < 3f && a.m_Health > 0f
            select a;
    }
    
    public List<TestCallFunction> AllEnemiesLowHealth2(List<TestCallFunction> sample)
    {
        List<TestCallFunction> aaa = new List<TestCallFunction>();

        for (int i = 0; i < sample.Count; i++)
        {
            if (sample[i].m_Health < 3f && sample[i].m_Health > 0f)
            {
                aaa.Add(sample[i]);
            }
        }

        return aaa;
    }
    
    public List<TestCallFunction> AllEnemiesLowHealth3(List<TestCallFunction> sample)
    {
        return sample.Where(x => x.m_Health < 3f && x.m_Health > 0f).OrderBy(t => t.m_Health).ToList();

        // for (int i = 0; i < sample.Count; i++)
        // {
        //     if (sample[i].m_Health < 3f && sample[i].m_Health > 0f)
        //     {
        //         aaa.Add(sample[i]);
        //     }
        // }
        //
        // return aaa;
    }
    
    public void Measure(Action _action, string aaa)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        _action();
        stopWatch.Stop();
        TimeSpan timeTaken = stopWatch.Elapsed;
        Helper.DebugLog("Time " + aaa + ": " + timeTaken.TotalMilliseconds);
    }
}

