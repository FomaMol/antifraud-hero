using System;
using System.Linq;
using Common.Extensions;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Widgets;

namespace Services
{
    public class GameplayService : IInitializable
    {
        [Inject] private readonly PersistingService _persistingService;
        
        private GameplayConfig _gameplayConfig;

        public int CurrentCaseIndex
        {
            get =>  _persistingService.CurrentCaseIndex;
            private set => _persistingService.SetCurrentCase(value);
        }
        public int CurrentShiftIndex
        {
            get => _persistingService.CurrentShiftIndex;
            private set => _persistingService.SetCurrentShift(value);
        }

        public CaseConfig CurrentCaseConfig => _gameplayConfig.Shifts[_persistingService.CurrentShiftIndex].Cases[CurrentCaseRealIndex];
        public int TotalCaseCountInCurrentShift => _gameplayConfig.Shifts[_persistingService.CurrentShiftIndex].Cases.Count;
        public int TotalShiftCount => _gameplayConfig.Shifts.Count;
        private int CurrentCaseRealIndex => _persistingService.CasesOrder[CurrentCaseIndex];

        private readonly Subject<Unit> _onChangeShift = new();
        public IObservable<Unit> OnChangeShift => _onChangeShift;
        
        private readonly Subject<Unit> _onChangeCase = new();
        public IObservable<Unit> OnChangeCase => _onChangeCase;

        public void Initialize()
        {
            var json = Resources.Load<TextAsset>("GameplayConfig").text;
            _gameplayConfig = JsonConvert.DeserializeObject<GameplayConfig>(json);
            if ((_persistingService.CasesOrder?.Count ?? 0) != TotalCaseCountInCurrentShift)
            {
                _persistingService.SetCases(Enumerable.Range(0, TotalCaseCountInCurrentShift).Shuffle().ToList());
            }
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
                    _persistingService.SetCases(Enumerable.Range(0, TotalCaseCountInCurrentShift).Shuffle().ToList());
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
                return _persistingService.CurrentShiftIndex + 1 >= _gameplayConfig.SenderToolOpenShift;
            if (toolType == ToolType.Db)
                return _persistingService.CurrentShiftIndex + 1 >= _gameplayConfig.DbToolOpenShift;
            
            throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null);
        }
    }
}