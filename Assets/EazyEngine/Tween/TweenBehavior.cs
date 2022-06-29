//using System;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyEngine.Core;
using UnityExtensions.Tween;

namespace  EazyEngine.Tween
{

    public class TweenBehavior : PersistentSingleton<TweenBehavior>
    {

        public List<AnimationInfo> runningAnimations = new List<AnimationInfo>();

        public void pushAnimation(AnimationInfo tween,GameObject rootObject,PlayDirection direction = PlayDirection.Forward)
        {
            tween.direction = direction;
           // if(direction =)
            tween.OnStart(rootObject);
            runningAnimations.Add(tween);
        }

        private void Update()
        {
            for (var i = runningAnimations.Count - 1; i >= 0; --i)
            {
                runningAnimations[i].OnUpdate();
                if ((runningAnimations[i]._state == 1 && runningAnimations[i].direction == PlayDirection.Forward) ||(runningAnimations[i]._state == -1 && runningAnimations[i].direction == PlayDirection.Back))
                {
                    runningAnimations.RemoveAt(i);
                }
            }
      
        }
    }

}
