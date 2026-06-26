using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    public static class ObjectExtensions
    {
        public static void SetDirty(this Object unityObject)
        {
            if (unityObject != null)
                EditorUtility.SetDirty(unityObject);
        }
    }
}