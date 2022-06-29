using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestSave : MonoBehaviour
{
    public List<CharacterSaveData> characterList;
    public CharacterDataSaveCollection characterSaveData;
    public List<CharacterSaveData> characterDefaultData;
    
    private void Start()
    {
        if (ES3.KeyExists(TagName.TestSave.testCharSave))
        {
            List<CharacterSaveData> charSaveData = ES3.Load<List<CharacterSaveData>>(TagName.TestSave.testCharSave);
            
            if (charSaveData.Count < characterSaveData.Value.Count) //Save data nho hon config data (update them item)
            {
                Helper.DebugLog("Update new item: " + (characterSaveData.Value.Count - charSaveData.Count));
                
                for (int i = charSaveData.Count; i < characterSaveData.Value.Count; i++)
                {
                    charSaveData.Add(characterSaveData.Value[i]);
                }
                
                ES3.Save<List<CharacterSaveData>>(TagName.TestSave.testCharSave, charSaveData);
            }
            else
            {
                characterSaveData.Value =
                    ES3.Load<List<CharacterSaveData>>(TagName.TestSave.testCharSave);  
            }
            // Helper.DebugLog("11111111111111111111");
        }
        else
        {
            characterSaveData.Value =
                ES3.Load<List<CharacterSaveData>>(TagName.TestSave.testCharSave, characterSaveData.Value);
            ES3.Save<List<CharacterSaveData>>(TagName.TestSave.testCharSave, characterSaveData.Value);
            // Helper.DebugLog("22222222222222222222");
        }
    }
    
    [Button]
    public void TestToSave()
    {
        // ES3.Save<List<CharacterSaveData>>(TagName.TestSave.testCharSave, characterList);
        ES3.Save<List<CharacterSaveData>>(TagName.TestSave.testCharSave, characterSaveData.Value);
    }
    
    [Button]
    public void TestToLoad()
    {
        characterSaveData.Value =
            ES3.Load<List<CharacterSaveData>>(TagName.TestSave.testCharSave);
        
        for (int i = 0; i < characterSaveData.Value.Count; i++)
        {
            Helper.DebugLog("ID: " + characterSaveData.Value[i].m_ID);
            Helper.DebugLog("Name: " + characterSaveData.Value[i].m_Name);
            Helper.DebugLog("Level: " + characterSaveData.Value[i].m_Level);
        }
    }

    // [Button]
    // public void TestToLoadNewData()
    // {
    //     for (int i = 0; i < characterSaveData.List.Count; i++)
    //     {
    //         Helper.DebugLog("ID: " + characterSaveData.List[i].);
    //         Helper.DebugLog("Name: " + characterSaveData.Value[i].m_Name);
    //         Helper.DebugLog("Level: " + characterSaveData.Value[i].m_Level);
    //     }
    // }
}
