using System;
using System.Collections.Generic;
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
        public IReadOnlyList<CaseConfig> CaseConfigsInCurrentShift => _persistingService.CasesOrder.Select(it => _gameplayConfig.Shifts[CurrentShiftIndex].Cases[it]).ToList();
        public int AvailableHelpsNumber => _persistingService.AvailableHelpsNumber;
        public bool HelpWasUsedInCurrentCase => _persistingService.HelpWasUsedInCurrentCase;

        private readonly Subject<Unit> _onChangeShift = new();
        public IObservable<Unit> OnChangeShift => _onChangeShift;
        
        private readonly Subject<Unit> _onChangeCase = new();
        public IObservable<Unit> OnChangeCase => _onChangeCase;
        
        private readonly Subject<Unit> _onUseHelp = new();
        public IObservable<Unit> OnUseHelp => _onUseHelp;

        public void Initialize()
        {
            var json = Resources.Load<TextAsset>("GameplayConfig").text;
            _gameplayConfig = JsonConvert.DeserializeObject<GameplayConfig>(json);
            if ((_persistingService.CasesOrder?.Count ?? 0) != TotalCaseCountInCurrentShift)
            {
                _persistingService.SetCasesOrder(Enumerable.Range(0, TotalCaseCountInCurrentShift).Shuffle().ToList());
            }
            if (_persistingService.CaseDecisionHistory == null)
            {
                _persistingService.SetCaseResultHistory(new List<bool>());
            }
            _persistingService.SetWasGameplayPlayed(true);
        }

        public void MakeChoice(bool isValid)
        {
            _persistingService.SetCaseResultHistory(_persistingService.CaseDecisionHistory.Append(isValid).ToList());
            _persistingService.SetHelpWasUsedInCurrentCase(false);
            
            ++CurrentCaseIndex;
            if (CurrentCaseIndex >= _gameplayConfig.Shifts[CurrentShiftIndex].Cases.Count)
            {
                CurrentCaseIndex = 0;
                _persistingService.SetCaseResultHistory(new List<bool>());
                _persistingService.SetCasesOrder(Enumerable.Range(0, TotalCaseCountInCurrentShift).Shuffle().ToList());
                _persistingService.SetAvailableHelpsNumber(3); // TODO: вынести в константы
                ++CurrentShiftIndex;
                if (CurrentShiftIndex >= _gameplayConfig.Shifts.Count)
                {
                    CurrentShiftIndex = 0;
                }
                _onChangeShift.OnNext(Unit.Default);
            }

            _onChangeCase.OnNext(Unit.Default);
        }

        public void TryUseHelp()
        {
            if (AvailableHelpsNumber > 0)
            {
                _persistingService.SetAvailableHelpsNumber(AvailableHelpsNumber - 1);
                _persistingService.SetHelpWasUsedInCurrentCase(true);
                _onUseHelp.OnNext(Unit.Default);
            }
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

        public bool TryGetCaseDecisionFromHistory(int caseIndex, out bool caseDecision)
        {
            caseDecision = false;
            if (_persistingService.CaseDecisionHistory.IndexOutOfBounds(caseIndex))
                return false;
            caseDecision = _persistingService.CaseDecisionHistory[caseIndex];
            return true;
        }

        public bool GetExpectedDecisionForCase(int caseIndex)
        {
            var caseConfig = CaseConfigsInCurrentShift[caseIndex];
            return caseConfig.LinkValidationResult == 1 
                   && caseConfig.SenderValidationResult == 1
                   && caseConfig.DbValidationResult == 1;
        }

        public float GetWinRateForCurrentShift()
        {
            if (_persistingService.CaseDecisionHistory.Count == 0)
                return 0;
            return _persistingService.CaseDecisionHistory
                       .Select((it, index) => it == GetExpectedDecisionForCase(index))
                       .Sum(it => it ? 1f : 0f) / _persistingService.CaseDecisionHistory.Count;
        }
    }
}