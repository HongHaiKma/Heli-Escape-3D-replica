using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/GunInventoryItem",fileName = "GunInventoryItem")]
public class GunInventoryItem : ScriptableObject
{
    // [PropertyOrder(-1)]
    [PreviewField]
    [ShowInInspector] 
    public Sprite img_Gun;
    
    public int m_ID;
    public string m_Name;
    public GameObject go_UIPrefabInventory;
}
