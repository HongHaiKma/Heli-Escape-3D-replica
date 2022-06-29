using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EazyEngine.UI
{
    
    public class BaseItem<T> : MonoBehaviour where T : class
    {
    
       // [SerializeField]
        protected T _data;
        public List<UnityEventObject> onDataAction = new List<UnityEventObject>();
        public List<UnityEventObject> onItemAction = new List<UnityEventObject>();
        public bool Using { get; set; }
        public T Data { get => _data; set { setData(value);  } }
        public bool Dirty { get; set; }
        public int Index { get; set; }
        public virtual void setData(T data)
        {
             this._data = data;
        }

        public virtual void executeFirst()
        {
            executeIndex(0);
        }
        public void executeSecond()
        {
            executeIndex(1);
        }
        public void executeIndex(int index)
        {
            if (index < onDataAction.Count)
            {
                onDataAction[index].Invoke(Data);
            }
            
            if (index < onItemAction.Count)
            {
                onItemAction[index].Invoke(this);
            }
        
        }
        public virtual void show(bool imediately = true)
        {
            gameObject.SetActive(true);
        }
        public virtual void hide()
        {
            gameObject.SetActive(false);
        }
    }
}
