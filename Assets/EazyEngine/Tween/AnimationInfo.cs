using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityExtensions;
using UnityExtensions.Tween;
using Object = UnityEngine.Object;
using WrapMode = UnityExtensions.Tween.WrapMode;

namespace  EazyEngine.Tween
{
        [System.Serializable]
    public class AnimationInfo
    {
        public string _id;
        [SerializeField]
        float _duration = 1f;

        /// <summary>
        /// Use unscaled delta time or normal delta time?
        /// </summary>
        public TimeMode timeMode = TimeMode.Unscaled;

        /// <summary>
        /// The wrap mode for playing.
        /// </summary>
        public UnityExtensions.Tween.WrapMode wrapMode = UnityExtensions.Tween.WrapMode.Clamp;

        /// <summary>
        /// Controls whether playback stops when the animation ends.
        /// </summary>
        public ArrivedAction arrivedAction = ArrivedAction.AlwaysStopOnArrived;

        /// <summary>
        /// The direction of the playback.
        /// </summary>
        [NonSerialized] public PlayDirection direction;

        [SerializeField] public UnityEvent _onForwardArrived = default;
        [SerializeField]public UnityEvent _onBackArrived = default;
        [SerializeField,SerializeReference]
        public List<TweenAnimation> _animations;
        float _normalizedTime = 0f;
        [NonSerialized]public int _state = 0;
        public float duration
        {
            get => _duration;
            set => _duration = value;
        }
        public float normalizedTime
        {
            get => _normalizedTime;
            set
            {
                _normalizedTime = Mathf.Clamp01(value);
                Sample(_normalizedTime);
            }
        }
        public int animationCount => _animations == null ? 0 : _animations.Count;
        
        public void Sample(float normalizedTime)
        {
            if (_animations != null)
            {
                for (int i = 0; i < _animations.Count; i++)
                {
                    var item = _animations[i];
                    if (item.enabled) item.Sample(normalizedTime);
                }
            }
        }
        protected  bool enabled = true;
        public  void OnUpdate()
        {

            float deltaTime = RuntimeUtilities.GetUnitedDeltaTime(timeMode);

            while ( enabled && deltaTime > Mathf.Epsilon)
            {
                if (direction == PlayDirection.Forward)
                {
                    if (_normalizedTime < 1f)
                    {
                        _state = 0;
                    }
                    else if (wrapMode == WrapMode.Loop)
                    {
                        _normalizedTime = 0f;
                        _state = 0;
                    }

                    float time = _normalizedTime * _duration + deltaTime;

                    // playing
                    if (time < _duration)
                    {
                        normalizedTime = time / _duration;
                        return;
                    }

                    // arrived
                    normalizedTime = 1f;
                    if (_state != +1)
                    {
                        _state = +1;

                        if ((arrivedAction & ArrivedAction.StopOnForwardArrived) != 0)
                            enabled = false;

                        _onForwardArrived?.Invoke();
                    }

                    // wrap
                    switch (wrapMode)
                    {
                        case WrapMode.Clamp:
                            return;

                        case WrapMode.PingPong:
                            direction = PlayDirection.Back;
                            break;
                    }

                    deltaTime = time - _duration;
                }
                else
                {
                    if (_normalizedTime > 0f)
                    {
                        _state = 0;
                    }
                    else if (wrapMode == WrapMode.Loop)
                    {
                        _normalizedTime = 1f;
                        _state = 0;
                    }

                    float time = _normalizedTime * _duration - deltaTime;

                    // playing
                    if (time > 0f)
                    {
                        normalizedTime = time / _duration;
                        return;
                    }

                    // arrived
                    normalizedTime = 0f;
                    if (_state != -1)
                    {
                        _state = -1;

                        if ((arrivedAction & ArrivedAction.StopOnBackArrived) != 0)
                            enabled = false;

                        _onBackArrived?.Invoke();
                    }

                    // wrap
                    switch (wrapMode)
                    {
                        case WrapMode.Clamp:
                            return;

                        case WrapMode.PingPong:
                            direction = PlayDirection.Forward;
                            break;
                    }

                    deltaTime = -time;
                }
            }
        }
        
 
        public void OnStart(Object rootObject)
        {
            _state = 0;
            enabled = true;
            foreach (var animation in _animations)
            {
                animation.setTarget(rootObject);
                animation.Reset();
            }
            
        }
    }
}
