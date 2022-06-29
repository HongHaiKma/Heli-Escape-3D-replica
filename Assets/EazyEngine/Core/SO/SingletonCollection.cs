using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ScriptableObjectArchitecture;
using Sirenix.Utilities;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif
using UnityEngine;

namespace EazyEngine.Core
{
    public class SingletonCollection<T,T1> : Collection<T1> where T : SingletonCollection<T,T1> 
    {
   
        private static T instance;
        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            SingletonCollection<T,T1>.instance = (T)this;
        }
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
                if (SingletonCollection<T,T1>.configAttribute == null)
                {
                    SingletonCollection<T,T1>.configAttribute = typeof(T).GetCustomAttribute<GlobalConfigAttribute>();
                    if (SingletonCollection<T,T1>.configAttribute == null)
                        SingletonCollection<T,T1>.configAttribute =
                            new GlobalConfigAttribute(TypeExtensions.GetNiceName(typeof(T)));
                }

                return SingletonCollection<T,T1>.configAttribute;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has instance loaded.
        /// </summary>
        public static bool HasInstanceLoaded
        {
            get { return (UnityEngine.Object) SingletonCollection<T,T1>.instance != (UnityEngine.Object) null; }
        }
#endif
        /// <summary>Gets the singleton instance.</summary>
        public static T Instance
        {
            get
            {
                #if UNITY_EDITOR
                if ((UnityEngine.Object) SingletonCollection<T,T1>.instance == (UnityEngine.Object) null)
                {
                    SingletonCollection<T,T1>.LoadInstanceIfAssetExists();
                    T instance = SingletonCollection<T,T1>.instance;
                    string str = Application.dataPath + "/" + SingletonCollection<T,T1>.ConfigAttribute.AssetPath +
                                 TypeExtensions.GetNiceName(typeof(T)) + ".asset";
                    if ((UnityEngine.Object) instance == (UnityEngine.Object) null &&
                        EditorPrefs.HasKey("PREVENT_SIRENIX_FILE_GENERATION"))
                    {
                        Debug.LogWarning((object) (SingletonCollection<T,T1>.ConfigAttribute.AssetPath +
                                                   TypeExtensions.GetNiceName(typeof(T)) +
                                                   ".asset was prevented from being generated because the PREVENT_SIRENIX_FILE_GENERATION key was defined in Unity's EditorPrefs."
                            ));
                        SingletonCollection<T,T1>.instance = ScriptableObject.CreateInstance<T>();
                        return SingletonCollection<T,T1>.instance;
                    }

                    if ((UnityEngine.Object) instance == (UnityEngine.Object) null && File.Exists(str) &&
                        EditorSettings.serializationMode == SerializationMode.ForceText)
                    {
                        if (AssetScriptGuidUtility.TryUpdateAssetScriptGuid(str, typeof(T)))
                        {
                            Debug.Log(
                                (object)
                                "Could not load config asset at first, but successfully detected forced text asset serialization, and corrected the config asset m_Script guid.");
                            SingletonCollection<T,T1>.LoadInstanceIfAssetExists();
                            instance = SingletonCollection<T,T1>.instance;
                        }
                        else
                            Debug.LogWarning(
                                (object)
                                "Could not load config asset, and failed to auto-correct config asset m_Script guid.");
                    }

                    if ((UnityEngine.Object) instance == (UnityEngine.Object) null)
                    {
                        instance = ScriptableObject.CreateInstance<T>();
                        if (!Directory.Exists(AssetPathWithAssetsPrefix(SingletonCollection<T,T1>.ConfigAttribute)))
                        {
                            Directory.CreateDirectory(
                                new DirectoryInfo(AssetPathWithAssetsPrefix(SingletonCollection<T,T1>.ConfigAttribute))
                                    .FullName);
                            AssetDatabase.Refresh();
                        }

                        string niceName = TypeExtensions.GetNiceName(typeof(T));
                        string path = !SingletonCollection<T,T1>.ConfigAttribute.AssetPath.StartsWith("Assets/")
                            ? "Assets/" + SingletonCollection<T,T1>.ConfigAttribute.AssetPath + niceName + ".asset"
                            : SingletonCollection<T,T1>.ConfigAttribute.AssetPath + niceName + ".asset";
                        if (File.Exists(str))
                        {
                            Debug.LogWarning((object) ("Could not load config asset of type " + niceName +
                                                       " from project path '" + path +
                                                       "', but an asset file already exists at the path, so could not create a new asset either. The config asset for '" +
                                                       niceName +
                                                       "' has been lost, probably due to an invalid m_Script guid. Set forced text serialization in Edit -> Project Settings -> Editor -> Asset Serialization -> Mode and trigger a script reload to allow Odin to auto-correct this."
                                ));
                        }
                        else
                        {
                            AssetDatabase.CreateAsset((UnityEngine.Object) instance, path);
                            AssetDatabase.SaveAssets();
                            SingletonCollection<T,T1>.instance = instance;
                            instance.OnConfigAutoCreated();
                            EditorUtility.SetDirty((UnityEngine.Object) instance);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }

                    SingletonCollection<T,T1>.instance = instance;
                }

 
                #endif
                return SingletonCollection<T,T1>.instance;
            }
        }
#if UNITY_EDITOR
        /// <summary>Tries to load the singleton instance.</summary>
        public static void LoadInstanceIfAssetExists()
        {
            if (SingletonCollection<T,T1>.ConfigAttribute.IsInResourcesFolder)
            {
                SingletonCollection<T,T1>.instance = Resources.Load<T>(
                    SingletonCollection<T,T1>.ConfigAttribute.ResourcesPath + TypeExtensions.GetNiceName(typeof(T)));
            }
            else
            {
                string niceName = TypeExtensions.GetNiceName(typeof(T));
                SingletonCollection<T,T1>.instance =
                    AssetDatabase.LoadAssetAtPath<T>(SingletonCollection<T,T1>.ConfigAttribute.AssetPath + niceName +
                                                     ".asset");
                if ((UnityEngine.Object) SingletonCollection<T,T1>.instance == (UnityEngine.Object) null)
                    SingletonCollection<T,T1>.instance = AssetDatabase.LoadAssetAtPath<T>(
                        "Assets/" + SingletonCollection<T,T1>.ConfigAttribute.AssetPath + niceName + ".asset");
            }

            if (!((UnityEngine.Object) SingletonCollection<T,T1>.instance == (UnityEngine.Object) null))
                return;
            string[] assets = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            if (assets.Length == 0)
                return;
            SingletonCollection<T,T1>.instance =
                AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[0]));
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
                ((IEnumerable<MethodInfo>) type.GetMethods())
                    .Where<MethodInfo>((Func<MethodInfo, bool>) (x =>
                        x.Name == "OpenWindow" && x.GetParameters().Length == 1)).First<MethodInfo>().Invoke(
                        (object) null, new object[1]
                        {
                            (object) this
                        });
            else
                Debug.LogError(
                    (object)
                    "Failed to open window, could not find Sirenix.OdinInspector.Editor.SirenixPreferencesWindow");
        }

        protected  virtual void OnConfigAutoCreated()
        {
        }
        #endif
    }
}