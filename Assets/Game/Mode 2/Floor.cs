using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public List<Enemy2> m_Enemies;
    public GameObject go_Blocks;

    public void RemoveEnemy(Enemy2 _enemy)
    {
        m_Enemies.Remove(_enemy);
    }
}