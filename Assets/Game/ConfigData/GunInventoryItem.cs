using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/GunInventoryItem",fileName = "GunInventoryItem")]
public class GunInventoryItem : ScriptableObject
{
    public int m_ID;
    public string m_Name;
    public Sprite img_Gun;
}
