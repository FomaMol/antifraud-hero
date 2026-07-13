using System;
using UnityEngine;

namespace Widgets.StatusBar
{
    public class StatusBarWidget : Widget
    {
        [SerializeField] private ShiftNumberWidget _shiftNumberWidget;
        [SerializeField] private ShiftProgressWidget _shiftProgressWidget;
        [SerializeField] private WinrateWidget _winrateWidget;
        
        public void Initialize()
        {
            _shiftNumberWidget.Initialize();
            _shiftProgressWidget.Initialize();
            _winrateWidget.Initialize();
        }

        private void Start()
        {
            Initialize();
        }
    }
}