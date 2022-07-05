using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public IntVariable m_Gold;

    [Title("Gun Inventory")]
    public GunSaveDataCollection m_GunSaveData_Mode1;
    public GunSaveDataCollection m_GunSaveData_Mode2;
    public GunSaveDataCollection m_GunSaveData_Mode3;

    public GunInventoryConfig m_GunConfig_Mode1;
    public GunInventoryConfig m_GunConfig_Mode2;
    public GunInventoryConfig m_GunConfig_Mode3;

    void OnEnable()
    {
        m_Gold.AddListener(SaveGold);
    }

    void OnDisable()
    {
        m_Gold.RemoveListener(SaveGold);
    }

    void OnDestroy()
    {
        m_Gold.RemoveListener(SaveGold);
    }

    private void Start()
    {
        if (ES3.KeyExists(TagName.m_Gold))
        {
            m_Gold.Value = ES3.Load<int>(TagName.m_Gold);
        }
        else
        {
            m_Gold.Value = ES3.Load<int>(TagName.m_Gold, m_Gold.Value);
            ES3.Save<int>(TagName.m_Gold, m_Gold);
        }

        if (!ES3.KeyExists(TagName.Inventory.m_CurrentGun_Mode1))
        {
            ES3.Save<int>(TagName.Inventory.m_CurrentGun_Mode1, 0);
        }
        if (!ES3.KeyExists(TagName.Inventory.m_CurrentGun_Mode2))
        {
            ES3.Save<int>(TagName.Inventory.m_CurrentGun_Mode2, 0);
        }
        if (!ES3.KeyExists(TagName.Inventory.m_CurrentGun_Mode3))
        {
            ES3.Save<int>(TagName.Inventory.m_CurrentGun_Mode3, 0);
        }


        if (ES3.KeyExists(TagName.Inventory.m_GunSaveData_Mode1))
        {
            m_GunSaveData_Mode1.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1);
        }
        else
        {
            m_GunSaveData_Mode1.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1, m_GunSaveData_Mode1.Value);
            ES3.Save<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1, m_GunSaveData_Mode1.Value);
        }

        if (ES3.KeyExists(TagName.Inventory.m_GunSaveData_Mode2))
        {
            m_GunSaveData_Mode2.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode2);
        }
        else
        {
            m_GunSaveData_Mode3.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode3, m_GunSaveData_Mode3.Value);
            ES3.Save<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode3, m_GunSaveData_Mode3.Value);
        }

        if (ES3.KeyExists(TagName.Inventory.m_GunSaveData_Mode3))
        {
            m_GunSaveData_Mode3.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode3);
        }
        else
        {
            m_GunSaveData_Mode3.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode3, m_GunSaveData_Mode3.Value);
            ES3.Save<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode3, m_GunSaveData_Mode3.Value);
        }
    }

    public void SaveGold()
    {
        ES3.Save<int>(TagName.m_Gold, m_Gold.Value);
        // m_Gold.Value = ES3.Load<int>(TagName.m_Gold);
    }

    public void SaveData()
    {
        ES3.Save<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1, m_GunSaveData_Mode1.Value);
        m_GunSaveData_Mode1.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode1);

        ES3.Save<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode2, m_GunSaveData_Mode2.Value);
        m_GunSaveData_Mode2.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode2);

        ES3.Save<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode3, m_GunSaveData_Mode3.Value);
        m_GunSaveData_Mode3.Value = ES3.Load<List<GunSaveData>>(TagName.Inventory.m_GunSaveData_Mode3);
    }

    [Button]
    public void Test()
    {
        GunSaveData newSave = new GunSaveData();
        newSave.m_ID = 1;
        m_GunSaveData_Mode1.Value.Add(newSave);

        SaveData();

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

        // int gunItemIndex = 2;
        //
        // var gunSave = m_GunSaveData_Mode1.Value.Find(x => x.m_ID == gunItemIndex);
        //
        // if (gunSave != null)
        //     Helper.DebugLog("Gun ID: " + gunSave.m_ID);

        // for (int i = 0; i < m_GunSaveData_Mode1.Value.Count; i++)
        // {
        //     Helper.DebugLog("ID: " + m_GunSaveData_Mode1.Value[i]);
        // }
    }
}
