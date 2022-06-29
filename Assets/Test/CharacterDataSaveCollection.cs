using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "CharacterSaveDataCollection.asset",
        menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "CharacterSavedData",
        // menuName = "SaveGame/CharacterSavedData",
        order = 1)]
    public class CharacterDataSaveCollection : Collection<CharacterSaveData>
    {
        
    }
}
