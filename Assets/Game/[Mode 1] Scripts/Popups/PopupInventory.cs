using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using EnhancedScrollerDemos.GridSelection;
using Sirenix.OdinInspector;

public class PopupInventory : UICanvas, IEnhancedScrollerDelegate
{
    [Title("Property")]
    public GameMode m_GameMode;
    public Button btn_NextLevel;
    
    [Title("Scroller")]
    public int numberOfCellsPerRow = 3;
    private SmallList<Data> _data;
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;

    private void Awake()
    {
        m_ID = UIID.POPUP_WIN;
        Init();
        
        
    }

    void Start()
        {
            // set the application frame rate.
            // this improves smoothness on some devices
            // Application.targetFrameRate = 60;

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

            // set up some simple data
            _data = new SmallList<Data>();
            for (var i = 0; i < 1000; i ++)
            {
                _data.Add(new Data() { someText = i.ToString() });
            }

            // tell the scroller to reload now that we have the data
            scroller.ReloadData();
        }

        /// <summary>
        /// This function handles the cell view's button click event
        /// </summary>
        /// <param name="cellView">The cell view that had the button clicked</param>
        private void CellViewSelected(RowCellView cellView)
        {
            if (cellView == null)
            {
                // nothing was selected
            }
            else
            {
                // get the selected data index of the cell view
                var selectedDataIndex = cellView.DataIndex;

                // loop through each item in the data list and turn
                // on or off the selection state. This is done so that
                // any previous selection states are removed and new
                // ones are added.
                for (var i = 0; i < _data.Count; i++)
                {
                    _data[i].Selected = (selectedDataIndex == i);
                }
            }
        }

        /// <summary>
        /// This tells the scroller the number of cells that should have room allocated.
        /// For this example, the count is the number of data elements divided by the number of cells per row (rounded up using Mathf.CeilToInt)
        /// </summary>
        /// <param name="scroller">The scroller that is requesting the data size</param>
        /// <returns>The number of cells</returns>
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
        }

        /// <summary>
        /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
        /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
        /// cell size will be the width.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell size</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <returns>The size of the cell</returns>
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 350f;
        }

        /// <summary>
        /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
        /// Some examples of this would be headers, footers, and other grouping cells.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
        /// <returns>The cell for the scroller to use</returns>
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // first, we get a cell from the scroller by passing a prefab.
            // if the scroller finds one it can recycle it will do so, otherwise
            // it will create a new cell.
            CellView cellView = scroller.GetCellView(cellViewPrefab) as CellView;

            // data index of the first sub cell
            var di = dataIndex * numberOfCellsPerRow;

            cellView.name = "Cell " + (di).ToString() + " to " + ((di) + numberOfCellsPerRow - 1).ToString();

            // pass in a reference to our data set with the offset for this cell
            cellView.SetData(ref _data, di, CellViewSelected);

            // return the cell to the scroller
            return cellView;
        }
        
        #endregion
}

public class UIGunInventoryData
{
    public string someText;

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