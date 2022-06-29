using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif
using UnityEngine;

namespace  EazyEngine.Core
{
    public class GlobalConfigSerialized<T> : SerializedScriptableObject where T : GlobalConfigSerialized<T>
    {
      
    private static T instance;
#if UNITY_EDITOR
      private static GlobalConfigAttribute configAttribute;
    static  string AssetPathWithAssetsPrefix(GlobalConfigAttribute configAtr)
    {
          
      string assetPath = configAtr.AssetPath;
      return assetPath.StartsWith("Assets/") ? assetPath : "Assets/" + assetPath;
            
    }

    static   string AssetPathWithoutAssetsPrefix(GlobalConfigAttribute configAtr)
    {
            
      string assetPath = configAtr.AssetPath;
      return assetPath.StartsWith("Assets/") ? assetPath.Substring("Assets/".Length) : assetPath;
            
    }
    private static GlobalConfigAttribute ConfigAttribute
    {
      get
      {
        if (GlobalConfigSerialized<T>.configAttribute == null)
        {
          GlobalConfigSerialized<T>.configAttribute = typeof (T).GetCustomAttribute<GlobalConfigAttribute>();
          if (GlobalConfigSerialized<T>.configAttribute == null)
            GlobalConfigSerialized<T>.configAttribute = new GlobalConfigAttribute(TypeExtensions.GetNiceName(typeof (T)));
        }
        return GlobalConfigSerialized<T>.configAttribute;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has instance loaded.
    /// </summary>
    public static bool HasInstanceLoaded
    {
      get
      {
        return (UnityEngine.Object) GlobalConfigSerialized<T>.instance != (UnityEngine.Object) null;
      }
    }
#endif
      protected override void OnAfterDeserialize()
      {
        base.OnAfterDeserialize();
        GlobalConfigSerialized<T>.instance = (T)this;
      }

      /// <summary>Gets the singleton instance.</summary>
    public static T Instance
    {
      get
      {
        #if UNITY_EDITOR
        if ((UnityEngine.Object) GlobalConfigSerialized<T>.instance == (UnityEngine.Object) null)
        {
          GlobalConfigSerialized<T>.LoadInstanceIfAssetExists();
          T instance = GlobalConfigSerialized<T>.instance;
          string str = Application.dataPath + "/" + GlobalConfigSerialized<T>.ConfigAttribute.AssetPath + TypeExtensions.GetNiceName(typeof (T)) + ".asset";
          if ((UnityEngine.Object) instance == (UnityEngine.Object) null && EditorPrefs.HasKey("PREVENT_SIRENIX_FILE_GENERATION"))
          {
            Debug.LogWarning((object) (GlobalConfigSerialized<T>.ConfigAttribute.AssetPath + TypeExtensions.GetNiceName(typeof (T)) + ".asset was prevented from being generated because the PREVENT_SIRENIX_FILE_GENERATION key was defined in Unity's EditorPrefs."));
            GlobalConfigSerialized<T>.instance = ScriptableObject.CreateInstance<T>();
            return GlobalConfigSerialized<T>.instance;
          }
          if ((UnityEngine.Object) instance == (UnityEngine.Object) null && File.Exists(str) && EditorSettings.serializationMode == SerializationMode.ForceText)
          {
            if (AssetScriptGuidUtility.TryUpdateAssetScriptGuid(str, typeof (T)))
            {
              Debug.Log((object) "Could not load config asset at first, but successfully detected forced text asset serialization, and corrected the config asset m_Script guid.");
              GlobalConfigSerialized<T>.LoadInstanceIfAssetExists();
              instance = GlobalConfigSerialized<T>.instance;
            }
            else
              Debug.LogWarning((object) "Could not load config asset, and failed to auto-correct config asset m_Script guid.");
          }
          if ((UnityEngine.Object) instance == (UnityEngine.Object) null)
          {
            instance = ScriptableObject.CreateInstance<T>();
            if (!Directory.Exists(AssetPathWithAssetsPrefix(GlobalConfigSerialized<T>.ConfigAttribute)))
            {
              Directory.CreateDirectory(
                new DirectoryInfo(AssetPathWithAssetsPrefix(GlobalConfigSerialized<T>.ConfigAttribute))
                  .FullName);
              AssetDatabase.Refresh();
            }

            string niceName = TypeExtensions.GetNiceName(typeof (T));
            string path = !GlobalConfigSerialized<T>.ConfigAttribute.AssetPath.StartsWith("Assets/") ? "Assets/" + GlobalConfigSerialized<T>.ConfigAttribute.AssetPath + niceName + ".asset" : GlobalConfigSerialized<T>.ConfigAttribute.AssetPath + niceName + ".asset";
            if (File.Exists(str))
            {
              Debug.LogWarning((object) ("Could not load config asset of type " + niceName + " from project path '" + path + "', but an asset file already exists at the path, so could not create a new asset either. The config asset for '" + niceName + "' has been lost, probably due to an invalid m_Script guid. Set forced text serialization in Edit -> Project Settings -> Editor -> Asset Serialization -> Mode and trigger a script reload to allow Odin to auto-correct this."));
            }
            else
            {
              AssetDatabase.CreateAsset((UnityEngine.Object) instance, path);
              AssetDatabase.SaveAssets();
              GlobalConfigSerialized<T>.instance = instance;
              instance.OnConfigAutoCreated();
              EditorUtility.SetDirty((UnityEngine.Object) instance);
              AssetDatabase.SaveAssets();
              AssetDatabase.Refresh();
            }
          }
          GlobalConfigSerialized<T>.instance = instance;
        }
        #endif
        return GlobalConfigSerialized<T>.instance;
      }
    }
#if UNITY_EDITOR
    /// <summary>Tries to load the singleton instance.</summary>
    public static void LoadInstanceIfAssetExists()
    {
      if (GlobalConfigSerialized<T>.ConfigAttribute.IsInResourcesFolder)
      {
        GlobalConfigSerialized<T>.instance = Resources.Load<T>(GlobalConfigSerialized<T>.ConfigAttribute.ResourcesPath + TypeExtensions.GetNiceName(typeof (T)));
      }
      else
      {
        string niceName = TypeExtensions.GetNiceName(typeof (T));
        GlobalConfigSerialized<T>.instance = AssetDatabase.LoadAssetAtPath<T>(GlobalConfigSerialized<T>.ConfigAttribute.AssetPath + niceName + ".asset");
        if ((UnityEngine.Object) GlobalConfigSerialized<T>.instance == (UnityEngine.Object) null)
          GlobalConfigSerialized<T>.instance = AssetDatabase.LoadAssetAtPath<T>("Assets/" + GlobalConfigSerialized<T>.ConfigAttribute.AssetPath + niceName + ".asset");
      }
      if (!((UnityEngine.Object) GlobalConfigSerialized<T>.instance == (UnityEngine.Object) null))
        return;
      string[] assets = AssetDatabase.FindAssets("t:" + typeof (T).Name);
      if (assets.Length == 0)
        return;
      GlobalConfigSerialized<T>.instance = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[0]));
    }

    /// <summary>
    /// Opens the config in a editor window. This is currently only used internally by the Sirenix.OdinInspector.Editor assembly.
    /// </summary>
    public void OpenInEditor()
    {
      System.Type type = (System.Type) null;
      try
      {
        Assembly assembly1 = (Assembly) null;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.GetName().Name == "Sirenix.OdinInspector.Editor")
          {
            assembly1 = assembly2;
            break;
          }
        }
        if (assembly1 != null)
          type = assembly1.GetType("Sirenix.OdinInspector.Editor.SirenixPreferencesWindow");
      }
      catch
      {
      }
      if (type != null)
        ((IEnumerable<MethodInfo>) type.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (x => x.Name == "OpenWindow" && x.GetParameters().Length == 1)).First<MethodInfo>().Invoke((object) null, new object[1]
        {
          (object) this
        });
      else
        Debug.LogError((object) "Failed to open window, could not find Sirenix.OdinInspector.Editor.SirenixPreferencesWindow");
    }
#endif
    protected virtual void OnConfigAutoCreated()
    {
    }

    }

}
