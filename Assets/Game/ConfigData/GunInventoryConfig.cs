using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Database/GunInventoryConfig",fileName = "GunInventoryConfig")]
public class GunInventoryConfig : ScriptableObject
{
    public List<GunInventoryItem> m_GunItem;
}