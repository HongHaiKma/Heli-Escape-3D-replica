using System;
using System.Collections;
using System.Collections.Generic;
using EazyEngine.Core;
using UnityEngine;

namespace  IdleHeroes
{
    [System.Serializable]
    public  struct VariantStyleInfo
    {
        public string styleName;
        public FeedBackAction feedBackAction;
    }
    public class VariantStyleObject : MonoBehaviour
    {
        public VariantStyleInfo[] datas;

        public void ExecuteStyle(string style)
        {
            if (Array.Exists(datas, x => x.styleName == style))
            {
                var data = Array.Find(datas, x => x.styleName == style);
                data.feedBackAction?.Play();
            }
        }
    }

}
