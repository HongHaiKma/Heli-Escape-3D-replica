using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Direction m_Dir;
}

public enum Direction
{
    LEFT,
    RIGHT,
    UP,
}