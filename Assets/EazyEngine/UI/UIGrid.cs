using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EazyEngine.UI
{
    public interface ILayoutGroup
    {
        void Reposition();
    }
    public enum Sorting
    {
        None,
        Alphabetic,
        Horizontal,
        Vertical,
        Custom,
    }
    [System.Serializable]
    public class VariantSizeWithQuantity
    {
        public int col;
        public float size;
    }
    public class UIGrid : MonoBehaviour, ILayoutGroup
    {
        public Vector2 cellSize;
        public VariantSizeWithQuantity[] variantCellSize;
        public int col = 1;
        public bool hideInactive;
        public Sorting sorting = Sorting.None;
        public Vector2 pivot = new Vector2(0.5f, 0.5f);
        public Vector2 alignment = new Vector2(0.5f, 0.5f);
        public System.Comparison<Transform> onCustomSort;

        public int CurrentCol
        {
            get
            {
                return GetChildList().Count % col;
            }
        }
        public Vector2 CellSize { get {
                if(variantCellSize != null)
                {
                    if(System.Array.Exists(variantCellSize, x => x.col == CurrentCol))
                    {
                        return new Vector2( System.Array.Find(variantCellSize, x => x.col == CurrentCol).size,cellSize.y);
                    }
                }
                return cellSize;
            }  set => cellSize = value; }

        private void Awake()
        {
            
        }
        public List<Transform> GetChildList()
        {
            Transform myTrans = transform;
            List<Transform> list = new List<Transform>();

            for (int i = 0; i < myTrans.childCount; ++i)
            {
                Transform t = myTrans.GetChild(i);

                if (!hideInactive || (t && t.gameObject.activeSelf))
                {
                   list.Add(t);
                }
            }

            // Sort the list using the desired sorting logic
            if (sorting != Sorting.None)
            {
                if (sorting == Sorting.Alphabetic) list.Sort(SortByName);
                else if (sorting == Sorting.Horizontal) list.Sort(SortHorizontal);
                else if (sorting == Sorting.Vertical) list.Sort(SortVertical);
                else if (onCustomSort != null) list.Sort(onCustomSort);
                else Sort(list);
            }

            return list;
        }
        void OnValidate() { if (!Application.isPlaying && gameObject.activeSelf) Reposition(); }

        // Various generic sorting functions
        static public int SortByName(Transform a, Transform b) { return string.Compare(a.name, b.name); }
        static public int SortHorizontal(Transform a, Transform b) { return a.localPosition.x.CompareTo(b.localPosition.x); }
        static public int SortVertical(Transform a, Transform b) { return b.localPosition.y.CompareTo(a.localPosition.y); }
        [ContextMenu("Execute")]
        public void Reposition()
        {
            var childs = GetChildList();
            for(int i = 0; i < childs.Count; ++i)
            {
                var width = (Mathf.Min(childs.Count, col) - 1) * (CellSize.x);
                var curentWidth = (Mathf.Min(childs.Count/col == i/col ?   childs.Count % col : col, col) - 1) * (CellSize.x);
                var startX = (width - curentWidth) * alignment.x;
                var height = ((childs.Count / col + ((childs.Count % col) != 0 ? 1 : 0))-1)*CellSize.y;
                childs[i].transform.localPosition = new Vector3(startX + (i % col) * CellSize.x,- (i / col) * CellSize.y, 0);
                childs[i].transform.localPosition -= new Vector3(width *pivot.x, -height * (1-pivot.y), 0);
            }
        }

        protected virtual void Sort(List<Transform> list) { }
    }
}
