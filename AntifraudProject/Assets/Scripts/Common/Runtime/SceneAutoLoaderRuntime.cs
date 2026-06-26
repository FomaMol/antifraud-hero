#if UNITY_EDITOR

using Common.Extensions;
using UnityEditor;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Runtime accessor for the Scene Auto Loader target scene.
    /// Uses PlayerPrefs to share value between editor and playmode.
    /// Note: In player builds, the project key may differ from editor depending on data path.
    /// </summary>
    public static class SceneAutoLoaderRuntime
    {
        private const string cTargetScenePrefKey = "SceneAutoLoader.TargetScene";

        public static bool HasTargetScene => !TargetScene.IsNullOrEmpty();
        
        public static string TargetScene
        {
            get => EditorPrefs.GetString(MakeProjectKey(cTargetScenePrefKey), string.Empty);
            set => EditorPrefs.SetString(MakeProjectKey(cTargetScenePrefKey), value);
        }

        public static string GetPrefsKey => MakeProjectKey(cTargetScenePrefKey);
        
        private static string MakeProjectKey(string key)
        {
            return $"{Application.dataPath.GetHashCode():X}.{key}";
        }
        
    }
}

#endif
