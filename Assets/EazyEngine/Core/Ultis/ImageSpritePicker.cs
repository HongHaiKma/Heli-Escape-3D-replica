using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace  EazyEngine.Core
{
    public class ImageSpritePicker : MonoBehaviour
    {
        public Sprite[] frames;
        public bool isPixelPerfect;
        protected Image cacheImage;
        public Image Image => cacheImage ? cacheImage : cacheImage = GetComponent<Image>();
        public void setFrameIndex(int index)
        {
            if(index < frames.Length)
             Image.sprite = frames[index];
            if (isPixelPerfect)
            {
                Image.SetNativeSize();
            }
        }
        public void setFrameIndex(bool index)
        {
            if((index ? 1: 0) < frames.Length)
                Image.sprite = frames[(index ? 1: 0)];
            if (isPixelPerfect)
            {
                Image.SetNativeSize();
            }
        }
    }

}
