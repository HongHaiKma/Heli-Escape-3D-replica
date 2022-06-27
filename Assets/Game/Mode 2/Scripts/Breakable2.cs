using System.Collections;
using System.Collections.Generic;
using Exploder.Utils;
using UnityEngine;

public class Breakable2 : MonoBehaviour, IBreakable2
{
    public void OnTrigger()
    {
        GetComponent<Rigidbody>().useGravity = true;
        ExploderSingleton.Instance.ExplodeObject(gameObject);
    }
}
