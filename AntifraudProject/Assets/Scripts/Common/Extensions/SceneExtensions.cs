using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace common.Extensions
{
    public static class SceneExtensions
    {
        public static T FindObjectOfType<T>(this Scene scene) where T : Component
        {
            return scene.FindObjectsOfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> FindObjectsOfType<T>(this Scene scene) where T : Component
        {
            return scene.GetRootGameObjects().SelectMany(it => it.GetComponentsInChildren<T>());
        }
    }
}