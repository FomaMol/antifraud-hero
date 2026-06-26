using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Common.Utility
{
    public class AnimatorEventListener : MonoBehaviour
    {
        public event Action<string> OnAnimatorEvent;
    
        [UsedImplicitly]
        public void ExecuteEvent(string eventName)
        {
            OnAnimatorEvent?.Invoke(eventName);    
        }
    }
}
