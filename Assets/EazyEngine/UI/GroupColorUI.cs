using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CacheGraphicColor
{
    public Graphic key;
    public Color color;
}
[ExecuteInEditMode]
public class GroupColorUI : MonoBehaviour
{
    public Graphic[] graphics;
    public bool mul;
    [SerializeField]
    private Color color;

    public List<CacheGraphicColor> cacheColor = new List<CacheGraphicColor>();
    public Color Color
    {
        get => color;
        set
        {
            color = value;
        
            foreach (var graphic in graphics)
            {
          
                if (mul)
                {
                    if (!cacheColor.Exists(x=>x.key == graphic))
                    {
                        cacheColor.Add(new CacheGraphicColor(){key =  graphic,color =  graphic.color});
                    }

                    graphic.color = cacheColor.Find(x => x.key == graphic).color * color;
                }
                else
                {
                    graphic.color = color;
                }
            }
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var graphic in graphics)
        {
          
            if (mul)
            {
                if (!cacheColor.Exists(x=>x.key == graphic))
                {
                    cacheColor.Add(new CacheGraphicColor(){key =  graphic,color =  graphic.color});
                }

                graphic.color = cacheColor.Find(x => x.key == graphic).color * color;
                UnityEditor.EditorUtility.SetDirty(this);
            }
            else
            {
                graphic.color = color;
            }
        }
    }
#endif
    private void OnEnable()
    {
        foreach (var graphic in graphics)
        {
          
            if (mul)
            {
                if (!cacheColor.Exists(x=>x.key == graphic))
                {
                    cacheColor.Add(new CacheGraphicColor(){key =  graphic,color =  graphic.color});
                }

                graphic.color = cacheColor.Find(x => x.key == graphic).color * color;
            }
            else
            {
                graphic.color = color;
            }
        }
    }
}
