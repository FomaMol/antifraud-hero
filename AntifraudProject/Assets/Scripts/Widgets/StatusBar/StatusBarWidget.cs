using System;
using UnityEngine;

namespace Widgets.StatusBar
{
    public class StatusBarWidget : Widget
    {
        [SerializeField] private ShiftNumberWidget _shiftNumberWidget;
        [SerializeField] private ShiftProgressWidget _shiftProgressWidget;
        
        public void Initialize()
        {
            _shiftNumberWidget.Initialize();
            _shiftProgressWidget.Initialize();
        }

        private void Start()
        {
            Initialize();
        }
    }
}