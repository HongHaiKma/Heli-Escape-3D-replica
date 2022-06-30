using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using System;
using EnhancedScrollerDemos.GridSelection;

public delegate void SelectedDelegate(UIGunInventoryItemRow rowCellView);
public class UIGunInventoryItem : EnhancedScrollerCellView
{
    public UIGunInventoryItemRow[] rowCellViews;

    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public void SetData(ref SmallList<UIGunInventoryData> data, int startingIndex, SelectedDelegate selected)
    {
        // loop through the sub cells to display their data (or disable them if they are outside the bounds of the data)
        for (var i = 0; i < rowCellViews.Length; i++)
        {
            var dataIndex = startingIndex + i;

            // if the sub cell is outside the bounds of the data, we pass null to the sub cell
            rowCellViews[i].SetData(dataIndex, dataIndex < data.Count ? data[dataIndex] : null, selected);
        }
    }
}
