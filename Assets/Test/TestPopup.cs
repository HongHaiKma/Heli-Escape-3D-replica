using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestPopup : MonoBehaviour
{
    [Button]
    public void OpenPopup()
    {
        PopupCaller.OpenPopup(UIID.POPUP_INVENTORY);
        // PopupInventory inventPop = PopupCaller.GetPopup(UIID.POPUP_INVENTORY) as PopupInventory;
    }
}
