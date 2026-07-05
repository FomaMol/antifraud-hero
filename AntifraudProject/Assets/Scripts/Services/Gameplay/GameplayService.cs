using System;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using Widgets;

namespace Services
{
    public class GameplayService : IInitializable
    {
        private GameplayConfig _gameplayConfig;
        
        public int CurrentShiftIndex { get; private set; }
        public int CurrentCaseIndex { get; private set; }
        public CaseConfig CurrentCaseConfig => _gameplayConfig.Shifts[CurrentShiftIndex].Cases[CurrentCaseIndex];
        public int TotalCaseCountInCurrentShift => _gameplayConfig.Shifts[CurrentShiftIndex].Cases.Count;
        public int TotalShiftCount => _gameplayConfig.Shifts.Count;

        private readonly Subject<Unit> _onChangeShift = new();
        public IObservable<Unit> OnChangeShift => _onChangeShift;
        
        private readonly Subject<Unit> _onChangeCase = new();
        public IObservable<Unit> OnChangeCase => _onChangeCase;

        public void Initialize()
        {
            var json = Resources.Load<TextAsset>("GameplayConfig").text;
            _gameplayConfig = JsonConvert.DeserializeObject<GameplayConfig>(json);  
        }

        public void MakeChoice(bool isValid)
        {
            ++CurrentCaseIndex;
            if (CurrentCaseIndex >= _gameplayConfig.Shifts[CurrentShiftIndex].Cases.Count)
            {
                CurrentCaseIndex = 0;
                ++CurrentShiftIndex;
                if (CurrentShiftIndex >= _gameplayConfig.Shifts.Count)
                {
                    CurrentShiftIndex = 0;
                }
                _onChangeShift.OnNext(Unit.Default);
            }

            _onChangeCase.OnNext(Unit.Default);
        }

        public bool IsToolAvailable(ToolType toolType)
        {
            if (toolType == ToolType.Link)
                return true;
            if (toolType == ToolType.Contact)
                return CurrentShiftIndex + 1 >= _gameplayConfig.SenderToolOpenShift;
            if (toolType == ToolType.Db)
                return CurrentShiftIndex + 1 >= _gameplayConfig.DbToolOpenShift;
            
            throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
        }
    }
}