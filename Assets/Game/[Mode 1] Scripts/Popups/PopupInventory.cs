using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using EnhancedScrollerDemos.GridSelection;
using Sirenix.OdinInspector;

public class PopupInventory : MonoBehaviour, IEnhancedScrollerDelegate
{
    [Title("Property")]
    public GameMode m_GameMode;
    public Button btn_NextLevel;
    
    [Title("Scroller")]
    public int numberOfCellsPerRow = 3;
    private SmallList<UIGunInventoryData> _data;
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;

    [Title("Gun Data")]
    public GunInventoryConfig m_GunConfig_Mode1;

    // private void Awake()
    // {
    //     m_ID = UIID.POPUP_WIN;
    //     Init();
    //     
    //     
    // }

    void Start()
        {
            // tell the scroller that this script will be its delegate
            scroller.Delegate = this;

            // load in a large set of data
            LoadData();
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

            GunInventoryConfig gunConfigs = m_GunConfig_Mode1;

            // set up some simple data
            _data = new SmallList<UIGunInventoryData>();
            for (var i = 0; i < gunConfigs.m_GunItem.Count; i ++)
            {
                // _data.data
                _data.Add(new UIGunInventoryData()
                {
                    someText = i.ToString(),
                    sprite_Gun = gunConfigs.m_GunItem[i].img_Gun
                });
            }

            // tell the scroller to reload now that we have the data
            scroller.ReloadData();
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
    public string someText;
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