using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using EnhancedScrollerDemos.GridSelection;
using Sirenix.OdinInspector;
using UI.ThreeDimensional;
using Cysharp.Threading.Tasks;

public class PopupInventory : UICanvas, IEnhancedScrollerDelegate
{
    [Title("Property")]
    public Button btn_NextLevel;

    [Title("Scroller")]
    public int numberOfCellsPerRow = 3;
    private SmallList<UIGunInventoryData> _data;
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;

    [Title("Gun Data")]
    public UIObject3D m_UIObject3D;
    public GunInventoryConfig gunConfigs;
    public GunInventoryConfig m_GunConfig_Mode1;
    public GunInventoryConfig m_GunConfig_Mode2;
    public GunInventoryConfig m_GunConfig_Mode3;

    public Image img_Fix3DObject;

    [Title("Buttons")]
    public GameObject go_BtnBuyGold;
    public GameObject go_BtnBuyAds;
    public GameObject go_BtnEquip;

    private void Awake()
    {
        m_ID = UIID.POPUP_INVENTORY;
        Init();
    }

    public override void Start()
    {
        base.Start();
        // tell the scroller that this script will be its delegate
        scroller.Delegate = this;

        // load in a large set of data
        LoadData();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        LoadData();

        int gunIndex = 0;
        if (GameManager.Instance.m_GameMode == GameMode.MODE_1)
        {
            gunIndex = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode1);
        }
        else if (GameManager.Instance.m_GameMode == GameMode.MODE_2)
        {
            gunIndex = ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode2);
        }
        SelectGun(gunIndex);
    }

    public void BuyGold()
    {
        go_BtnBuyGold.SetActive(false);
    }

    public void BuyAds()
    {
        go_BtnBuyAds.SetActive(false);
    }

    public void Equip()
    {
        go_BtnEquip.SetActive(false);
    }

    public void SelectGun(int _index)
    {
        m_UIObject3D.ObjectPrefab = gunConfigs.m_GunItem[_index].go_UIPrefabInventory.transform;
        img_Fix3DObject.color = Color.white;
    }

    #region EnhancedScroller Handlers

    /// <summary>
    /// Populates the data with a lot of records
    /// </summary>
    private void LoadData()
    {
        // if the data existed previously, loop through
        // and remove the selection change handlers before
        // clearing out the data.
        if (_data != null)
        {
            for (var i = 0; i < _data.Count; i++)
            {
                _data[i].selectedChanged = null;
            }
        }

        // await UniTask.WaitUntil(() => GameManager.Instance != null);

        // GunInventoryConfig gunConfigs = new GunInventoryConfig();
        if (GameManager.Instance.m_GameMode == GameMode.MODE_1)
        {
            gunConfigs = m_GunConfig_Mode1;
        }
        else if (GameManager.Instance.m_GameMode == GameMode.MODE_2)
        {
            gunConfigs = m_GunConfig_Mode2;
        }
        else if (GameManager.Instance.m_GameMode == GameMode.MODE_3)
        {
            gunConfigs = m_GunConfig_Mode3;
        }



        // set up some simple data
        _data = new SmallList<UIGunInventoryData>();
        for (var i = 0; i < gunConfigs.m_GunItem.Count; i++)
        {
            _data.Add(new UIGunInventoryData()
            {
                m_ID = gunConfigs.m_GunItem[i].m_ID,
                sprite_Gun = gunConfigs.m_GunItem[i].img_Gun,
            });
        }

        // tell the scroller to reload now that we have the data
        scroller.ReloadData();

        for (var i = 0; i < _data.Count; i++)
        {
            if (GameManager.Instance.m_GameMode == GameMode.MODE_1)
            {
                // EventManager1<int>.CallEvent(GameEvent.SELECT_GUN, ES3.Load<int>(TagName.Inventory.m_CurrentGun_Mode1));
                EventManager1<int>.CallEvent(GameEvent.SELECT_GUN, _data[i].m_ID);
            }
            else if (GameManager.Instance.m_GameMode == GameMode.MODE_2)
            {
                EventManager1<int>.CallEvent(GameEvent.SELECT_GUN, _data[i].m_ID);
            }
            else if (GameManager.Instance.m_GameMode == GameMode.MODE_3)
            {
                EventManager1<int>.CallEvent(GameEvent.SELECT_GUN, _data[i].m_ID);
            }
        }
    }

    [Button]
    public void ReloadData()
    {
        LoadData();
    }

    /// <summary>
    /// This function handles the cell view's button click event
    /// </summary>
    /// <param name="cellView">The cell view that had the button clicked</param>
    private void CellViewSelected(UIGunInventoryItemRow cellView)
    {
        if (cellView == null)
        {
        }
        else
        {
            var selectedDataIndex = cellView.DataIndex;

            for (var i = 0; i < _data.Count; i++)
            {
                _data[i].Selected = (selectedDataIndex == i);
            }
        }
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 350f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        UIGunInventoryItem cellView = scroller.GetCellView(cellViewPrefab) as UIGunInventoryItem;

        var di = dataIndex * numberOfCellsPerRow;

        cellView.SetData(ref _data, di, CellViewSelected);

        return cellView;
    }

    #endregion
}

[System.Serializable]
public class UIGunInventoryData
// public class Data
{
    public int m_ID;
    public Sprite sprite_Gun;

    public SelectedChangedDelegate selectedChanged;

    /// <summary>
    /// The selection state
    /// </summary>
    private bool _selected;
    public bool Selected
    {
        get { return _selected; }
        set
        {
            // if the value has changed
            if (_selected != value)
            {
                // update the state and call the selection handler if it exists
                _selected = value;
                if (selectedChanged != null) selectedChanged(_selected);
            }
        }
    }
}