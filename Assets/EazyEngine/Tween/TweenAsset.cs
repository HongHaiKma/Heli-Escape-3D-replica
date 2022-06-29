using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityExtensions.Tween;

namespace EazyEngine.Tween
{
    public class TweenAsset : SerializedScriptableObject
    {
        [System.NonSerialized][OdinSerialize]
        public AnimationInfo _animation;
    }
}


