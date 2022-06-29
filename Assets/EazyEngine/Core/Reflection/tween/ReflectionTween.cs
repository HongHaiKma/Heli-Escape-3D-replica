using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityExtensions;
using UnityExtensions.Tween;

[Serializable, TweenAnimation("Reflection/Float","Reflection Float")]
public class FloatReflectionTween : TweenUnmanaged<float>
{
    [Filter(typeof(float), Fields =  true,Properties =  true  )]
    public UnityMember valueFloat;
    [Filter(typeof(float), Fields =  true,Properties =  true  )]
    public UnityMember[] extraValue;

    public override float Addictive(float a, float b)
    {
        return a + b;
    }

    public override float current
    {
        get
        {
            return valueFloat.target ?  valueFloat.Get<float>() : default;
        }
        set
        {
            if(valueFloat.target)
                valueFloat.Set(value);
            foreach (var extra in extraValue)
            {
                if(extra.target)
                    extra.Set(value);
            }
        }
    }
#if UNITY_EDITOR
    protected override void OnPropertiesGUI(TweenPlayer player, SerializedProperty property)
    {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("valueFloat"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("extraValue"));
        base.OnPropertiesGUI(player, property);
        var (runtimeFromProp,fromProp, toProp) = GetFromToProperties(property);
        EditorGUILayout.PropertyField(runtimeFromProp);
        var disableFrom = runtimeFromProp.boolValue;
        FromToFieldLayout("Value", fromProp, toProp,disableFrom);
    }
    #endif

    public override void Interpolate(float factor)
    {
        current = (destiny - From) * factor + From;
    }
}
[Serializable, TweenAnimation("Reflection/Vector3","Reflection Vector3")]
public class Vector3ReflectionTween : TweenUnmanaged<Vector3>
{
    [Filter(typeof(Vector3), Fields =  true,Properties =  true  )]
    public UnityMember valueVector3;
    [Filter(typeof(Vector3), Fields =  true,Properties =  true  )]
    public UnityMember[] extraValue;
    public bool3 toggle;
    public override Vector3 Addictive(Vector3 a, Vector3 b)
    {
        return a + b;
    }

    public override Vector3 current
    {
        get
        {
            return valueVector3.target ?  valueVector3.Get<Vector3>() : default;
        }
        set
        {
            if(valueVector3.target)
                valueVector3.Set(value);
            foreach (var extra in extraValue)
            {
                if(extra.target)
                    extra.Set(value);
            }
        }
    }
#if UNITY_EDITOR
    protected override void OnPropertiesGUI(TweenPlayer player, SerializedProperty property)
    {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("valueVector3"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("extraValue"));
        base.OnPropertiesGUI(player, property);
        var (runtimeFromProp, fromProp, toProp) = GetFromToProperties(property);
        EditorGUILayout.PropertyField(runtimeFromProp);
        var fromDisable = runtimeFromProp.boolValue;
        var toggleProp = property.FindPropertyRelative(nameof(toggle));

        FromToFieldLayout("X",
            fromProp.FindPropertyRelative(nameof(Vector3.x)),
            toProp.FindPropertyRelative(nameof(Vector3.x)),
            toggleProp.FindPropertyRelative(nameof(bool3.x)),fromDisable);
        FromToFieldLayout("Y",
            fromProp.FindPropertyRelative(nameof(Vector3.y)),
            toProp.FindPropertyRelative(nameof(Vector3.y)),
            toggleProp.FindPropertyRelative(nameof(bool3.y)),fromDisable);
        FromToFieldLayout("Z",
            fromProp.FindPropertyRelative(nameof(Vector3.z)),
            toProp.FindPropertyRelative(nameof(Vector3.z)),
            toggleProp.FindPropertyRelative(nameof(bool3.z)),fromDisable);
    }
    #endif

    public override void Interpolate(float factor)
    {
        current = (destiny - From) * factor + From;
    }
}

[Serializable, TweenAnimation("Reflection/Vector2","Reflection Vector2")]
public class Vector2ReflectionTween : TweenUnmanaged<Vector2>
{
    [Filter(typeof(Vector2), Fields =  true,Properties =  true  )]
    public UnityMember valueVector2;
    [Filter(typeof(Vector2), Fields =  true,Properties =  true  )]
    public UnityMember[] extraValue;
    public bool2 toggle;
    public override Vector2 Addictive(Vector2 a, Vector2 b)
    {
        return a + b;
    }

    public override Vector2 current
    {
        get
        {
            return valueVector2.target ?  valueVector2.Get<Vector2>() : default;
        }
        set
        {
            if(valueVector2.target)
                valueVector2.Set(value);
            foreach (var extra in extraValue)
            {
                if(extra.target)
                    extra.Set(value);
            }
        }
    }
#if UNITY_EDITOR
    protected override void OnPropertiesGUI(TweenPlayer player, SerializedProperty property)
    {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("valueVector2"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("extraValue"));
        base.OnPropertiesGUI(player, property);
        var (runtimeFromProp,fromProp, toProp) = GetFromToProperties(property);
        EditorGUILayout.PropertyField(runtimeFromProp);
        var disableFrom = runtimeFromProp.boolValue;
        var toggleProp = property.FindPropertyRelative(nameof(toggle));

        FromToFieldLayout("X",
            fromProp.FindPropertyRelative(nameof(Vector2.x)),
            toProp.FindPropertyRelative(nameof(Vector2.x)),
            toggleProp.FindPropertyRelative(nameof(bool2.x)),disableFrom);
        FromToFieldLayout("Y",
            fromProp.FindPropertyRelative(nameof(Vector2.y)),
            toProp.FindPropertyRelative(nameof(Vector2.y)),
            toggleProp.FindPropertyRelative(nameof(bool2.y)),disableFrom);
    }
    #endif
    

    public override void Interpolate(float factor)
    {
        current = (destiny - From) * factor + From;
    }
}