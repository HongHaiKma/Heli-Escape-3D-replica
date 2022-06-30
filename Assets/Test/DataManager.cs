using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [Title("Gun Inventory")]
    public GunSaveDataCollection m_GunSaveData_Mode1;
    public GunSaveDataCollection m_GunSaveData_Mode2;
    public GunSaveDataCollection m_GunSaveData_Mode3;

    public GunInventoryConfig m_GunConfig_Mode1;
    public GunInventoryConfig m_GunConfig_Mode2;
    public GunInventoryConfig m_GunConfig_Mode3;

    private void Start()
    {
        if (ES3.KeyExists(TagName.Inventory.m_GunSaveData_Mode1))
        {
            m_GunSaveData_Mode1.Value =
                ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1);
            // Helper.DebugLog("11111111111111111111");
        }
        else
        {
            m_GunSaveData_Mode1.Value =
                ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1, m_GunSaveData_Mode1.Value);
            ES3.Save<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1, m_GunSaveData_Mode1.Value);
            // Helper.DebugLog("22222222222222222222");
        }
    }

    [Button]
    public void Test()
    {
        // List<GunInventoryItem> gunItems = m_GunConfig_Mode1.m_GunItem;
        // for (int i = 0; i < gunItems.Count; i++)
        // {
        //     // var a = m_GunSaveData_Mode1.Value.Exists(x => x.m_ID == gunItems[i].m_ID);
        //     var a = m_GunSaveData_Mode1.Value.Exists(x => x.m_ID == gunItems[i].m_ID);
        //     if (a)
        //     {
        //         Helper.DebugLog("Exits");
        //     }
        //     else
        //     {
        //         Helper.DebugLog("Not Exits");
        //     }
        // }

        int gunItemIndex = 2;

        var gunSave = m_GunSaveData_Mode1.Value.Find(x => x.m_ID == gunItemIndex);

        if (gunSave != null)
            Helper.DebugLog("Gun ID: " + gunSave.m_ID);

        // for (int i = 0; i < m_GunSaveData_Mode1.Value.Count; i++)
        // {
        //     Helper.DebugLog("ID: " + m_GunSaveData_Mode1.Value[i]);
        // }
    }
}
