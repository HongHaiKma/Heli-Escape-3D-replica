using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public List<Enemy2> m_Enemies;
    public Breakable2[] m_Block;

    public void RemoveEnemy(Enemy2 _enemy)
    {
        m_Enemies.Remove(_enemy);
    }

    public void ActivateFloor()
    {
        for (int i = 0; i < m_Block.Length; i++)
        {
            m_Block[i].ActivateFloor();
        }
    }
}