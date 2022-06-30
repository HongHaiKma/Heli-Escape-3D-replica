using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "GunSaveData.asset",
        menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "GunSaveData",
        // menuName = "SaveGame/CharacterSavedData",
        order = 2)]
    public class GunSaveDataCollection : Collection<GunSaveData>
    {
    }
}

[System.Serializable]
public class GunSaveData
{
    public int m_ID;
}
