using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace EazyEngine.UI
{
    [System.Serializable]
    public class UnityEventObject : UnityEvent<object>
    {

    }
    public class BaseBox<TItem, TData> : MonoBehaviour where TItem : BaseItem<TData> where TData : class
    {
        public bool fixedDataArray;
        [ShowIf("fixedDataArray")]
        public int limitDataSize = 10;
        public GameObject attachMent;
        public TItem prefabItem;
        public List<UnityEventObject> onDataAction = new List<UnityEventObject>();
        public List<UnityEventObject> onItemClick = new List<UnityEventObject>();
        public List<TData> _infos = new List<TData>();
        public List<TItem> items = new List<TItem>();
        public TItem obtainItemExistData(TData pData)
        {
         
            for (int i = 0; i < Items.Count; ++i)
            {
                if (Items[i].Data == pData)
                {
                    Items[i].Using = true;
                    if (Items[i].Dirty)
                    {
                        showNewItem(Items[i]);
                    }
                    return Items[i];
                }
            }
            return null;
        }

        public TItem obtainItemNewData(TData pData, int index)
        {
            for (int i = 0; i < Items.Count; ++i)
            {
                if (!Items[i].Using)
                {
                    Items[i].Index = index;
                    Items[i].Dirty = true;
                    setDataItem(pData, Items[i]);
                    Items[i].onDataAction = onDataAction;
                    Items[i].onItemAction = onItemClick;
                    showNewItem(Items[i]);
                    return Items[i];
                }
            }

            var pItem = Instantiate<TItem>(prefabItem, attachMent.transform);
            pItem.Dirty = true;
            pItem.Index = index;
            setDataItem(pData, pItem);
            showNewItem(pItem);
            pItem.onDataAction = onDataAction;
            pItem.onItemAction = onItemClick;
            Items.Add(pItem);
            return pItem;
        }
        public virtual void setDataItem(TData pData, TItem pItem)
        {
            pItem.Data = pData;
        }
        public virtual void showNewItem(TItem pItem)
        {

        }

        public virtual List<TData> DataSource
        {
            set
            {
                _infos = value;
                repaint();
            }
            get
            {
                return _infos;
            }
        }

        public List<TItem> Items { get => items; set => items = value; }
        public Dictionary<TData, TItem> Item = new Dictionary<TData, TItem>();

        public List<TItem> GetItemsActive()
        {
            return Items.FindAll(x => x.gameObject.activeSelf);
        }
        public void repaint()
        {
            Item.Clear();
            for (int i = 0; i < Items.Count; ++i)
            {
                Items[i].Dirty = !Items[i].Using;
                Items[i].Using = false;
                Items[i].hide();
            }

            
            for (var i = DataSource.Count - 1; i >= 0; --i)
            {
                if (DataSource[i] == null)
                {
                    continue;
                }
                var pItem = obtainItemExistData(DataSource[i]);
                if (pItem)
                {
                    pItem.Using = true;
                }
            }
            for (var i = 0; i < DataSource.Count; ++i)
            {
                var pItem =DataSource[i] != null ? obtainItemExistData(DataSource[i]) : null;
                if (pItem)
                {

                    if (!Item.ContainsKey(DataSource[i]))
                    {
                        Item.Add(DataSource[i], pItem);
                    }
                    else
                    {
                        Item[DataSource[i]] = pItem;
                    }

                    pItem.Index = i;
                    setDataItem(DataSource[i], pItem);
                    pItem.onDataAction = onDataAction;
                    pItem.onItemAction = onItemClick;
                    pItem.show(!isItemEffect());
                    pItem.Using = true;
                }
                else
                {
                    pItem = obtainItemNewData(DataSource[i], i);
                    if (DataSource[i] != null)
                    {
                        if (!Item.ContainsKey(DataSource[i]))
                        {
                            Item.Add(DataSource[i], pItem);
                        }
                        else
                        {
                            Item[DataSource[i]] = pItem;
                        }
                    }

                    pItem.show(!isItemEffect());
                    pItem.Using = true;
                }
            }
            
   

            if (attachMent)
            {
                resortItem();
            }
        }
        public virtual bool isItemEffect()
        {
            return false;
        }
        public virtual void resortItem()
        {
            var layouts = attachMent.GetComponents<MonoBehaviour>().OfType<ILayoutGroup>();
            foreach(var layout in layouts)
            {
                layout.Reposition();
            }
        }
    }
}
