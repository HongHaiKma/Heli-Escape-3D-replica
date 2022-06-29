using System.Collections;
using System.Collections.Generic;
using EazyEngine.Timer;
using UnityEngine;
using UnityEngine.Events;

namespace  EazyEngine.Core
{
    public   class FeedBackAction : TimeControlBehavior
    {
        public UnityEvent onPlayEvent;
        public UnityEvent onStopEvent;
        [ContextMenu("Play")]
        public void Play()
        {
            onPlayEvent.Invoke();
        }
        [ContextMenu("Stop")]
        public void Stop()
        {
            onStopEvent.Invoke();
        }
    }
}

