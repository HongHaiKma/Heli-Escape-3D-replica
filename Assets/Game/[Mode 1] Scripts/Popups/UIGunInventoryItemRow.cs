using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using System;
using EnhancedScrollerDemos.GridSelection;
using UnityEngine.Serialization;

public class UIGunInventoryItemRow : MonoBehaviour
{
    public int m_ID;
    public Image img_Gun;
    public Image img_BG;
    // public Image selectionPanel;

    /// <summary>
    /// These are the colors for the selection of the cells
    /// </summary>
    // public Color selectedColor;
    // public Color unSelectedColor;

    /// <summary>
    /// Public reference to the index of the data
    /// </summary>
    public int DataIndex { get; private set; }

    /// <summary>
    /// The handler to call when this cell's button traps a click event
    /// </summary>
    public SelectedDelegate selected;

    /// <summary>
    /// Reference to the underlying data driving this view
    /// </summary>
    private UIGunInventoryData _data;

    /// <summary>
    /// This is called if the cell is destroyed. The EnhancedScroller will
    /// not call this since it uses recycling, but we include it in case 
    /// the user decides to destroy the cell anyway
    /// </summary>

    private void OnEnable()
    {
        EventManager1<int>.AddListener(GameEvent.SELECT_GUN, OnSelectGun);
    }

    private void OnDisable()
    {
        EventManager1<int>.RemoveListener(GameEvent.SELECT_GUN, OnSelectGun);
    }

    void OnDestroy()
    {
        EventManager1<int>.RemoveListener(GameEvent.SELECT_GUN, OnSelectGun);
        if (_data != null)
        {
            // remove the handler from the data so 
            // that any changes to the data won't try
            // to call this destroyed view's function
            _data.selectedChanged -= SelectedChanged;
        }
    }

    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public void SetData(int dataIndex, UIGunInventoryData data, SelectedDelegate selected)
    {
        // set the selected delegate
        this.selected = selected;

        // this cell was outside the range of the data, so we disable the img_Gun.
        // Note: We could have disable the cell gameobject instead of a child img_Gun,
        // but that can cause problems if you are trying to get components (disabled objects are ignored).
        img_Gun.gameObject.SetActive(data != null);
        img_BG.gameObject.SetActive(data != null);

        if (data != null)
        {
            // set the text if the cell is inside the data range
            m_ID = data.m_ID;
            img_Gun.sprite = data.sprite_Gun;
        }

        // if there was previous data assigned to this cell view,
        // we need to remove the handler for the selection change
        if (_data != null)
        {
            _data.selectedChanged -= SelectedChanged;
        }

        // link the data to the cell view
        DataIndex = dataIndex;
        _data = data;

        if (data != null)
        {
            // set up a handler so that when the data changes
            // the cell view will update accordingly. We only
            // want a single handler for this cell view, so 
            // first we remove any previous handlers before
            // adding the new one
            _data.selectedChanged -= SelectedChanged;
            _data.selectedChanged += SelectedChanged;

            // update the selection state UI
            SelectedChanged(data.Selected);
        }
    }

    public void OnSelectGun(int _id)
    {
        bool selected = false;
        if (GameManager.Instance.m_GameMode == GameMode.MODE_1)
        {
            selected = (m_ID == ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode1));
        }
        else if (GameManager.Instance.m_GameMode == GameMode.MODE_2)
        {
            selected = (m_ID == ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode2));
        }
        else if (GameManager.Instance.m_GameMode == GameMode.MODE_3)
        {
            selected = (m_ID == ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode3));
        }
        img_BG.color = (selected ? Helper.ConvertColor(new Color(47f, 255f, 53f, 255f)) : Helper.ConvertColor(new Color(77f, 77f, 77f, 255f)));
    }

    /// <summary>
    /// This function changes the UI state when the item is 
    /// selected or unselected.
    /// </summary>
    /// <param name="selected">The selection state of the cell</param>
    private void SelectedChanged(bool selected)
    {
        // selectionPanel.color = (selected ? selectedColor : unSelectedColor);
        // Helper.DebugLog("GGGGGGGGGGG");
        // if (true)
        // {
        //     Helper.DebugLog("GGGGGGGGGGG");
        // }
        // img_BG.color = (selected ? Helper.ConvertColor(new Color(77f, 77f, 77f, 255f)) : Helper.ConvertColor(new Color(47f, 255f, 53f, 255f)));
    }

    /// <summary>
    /// This function is called by the cell's button click event
    /// </summary>
    public void OnSelected()
    {
        // if a handler exists for this cell, then
        // call it.

        // if (selected != null)
        // {
        //     selected(this);
        //     Helper.DebugLog("AAAAAAAAAAAA");
        // }

        PopupInventory popInventory = PopupCaller.GetPopup(UIID.POPUP_INVENTORY) as PopupInventory;
        popInventory.m_CurGunID = m_ID;
        popInventory.SelectGun(m_ID);

        // if (GameManager.Instance.m_GameMode == GameMode.MODE_1)
        // {
        //     ES3.Save<int>(TagName.Inventory.m_CurrentGun_Mode1, m_ID);
        //     int curGunMode1 = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode1);
        //     GunInventoryConfig gunConfig = popInventory.gunConfigs;
        //     var gunInvent = gunConfig.m_GunItem.Find(x => x.m_ID == curGunMode1);
        //     CamController.Instance.SpawnGun(gunInvent);
        // }
        // if (GameManager.Instance.m_GameMode == GameMode.MODE_2)
        // {
        //     ES3.Save<int>(TagName.Inventory.m_CurrentGun_Mode2, m_ID);
        //     int curGunMode2 = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode2);
        //     GunInventoryConfig gunConfig = popInventory.gunConfigs;
        //     var gunInvent = gunConfig.m_GunItem.Find(x => x.m_ID == curGunMode2);
        //     CamController2.Instance.SpawnGun(gunInvent);
        // }
        // if (GameManager.Instance.m_GameMode == GameMode.MODE_3)
        // {
        //     ES3.Save<int>(TagName.Inventory.m_CurrentGun_Mode3, m_ID);
        //     int curGunMode3 = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode3);
        //     GunInventoryConfig gunConfig = popInventory.gunConfigs;
        //     var gunInvent = gunConfig.m_GunItem.Find(x => x.m_ID == curGunMode3);
        //     PlayerShootingController.Instance.SpawnGun(gunInvent);
        // }

        // UpdateUI();
    }

    public void UpdateUI()
    {
        if (GameManager.Instance.m_GameMode == GameMode.MODE_1)
        {
            EventManager1<int>.CallEvent(GameEvent.SELECT_GUN, m_ID);
        }
        else if (GameManager.Instance.m_GameMode == GameMode.MODE_2)
        {
            EventManager1<int>.CallEvent(GameEvent.SELECT_GUN, m_ID);
        }
        else if (GameManager.Instance.m_GameMode == GameMode.MODE_3)
        {
            EventManager1<int>.CallEvent(GameEvent.SELECT_GUN, m_ID);
        }
    }
}
