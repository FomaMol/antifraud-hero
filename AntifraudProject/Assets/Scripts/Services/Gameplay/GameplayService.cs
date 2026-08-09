using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Cysharp.Threading.Tasks;
using GameLogic.UI.Popups;
using Newtonsoft.Json;
using Popups;
using Services.Scenes;
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
        [Inject] private readonly PopupService _popupService;
        [Inject] private readonly ScenesService _scenesService;
 
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

            if (!_persistingService.WasGameplayPlayed)
            {
                _popupService.OpenPopup<IHowToPlayPopupPresenter>(new HowToPlayPopupPresenter(), queued: true).Forget();
                ForceProgressTo(0, 0);
            }
            else
                TryShowNewToolPopupForCurrentShift();
        }

        private void ForceProgressTo(int shiftIndex, int caseIndex)
        {
            _persistingService.ResetGameplayProgress();
            _persistingService.SetWasGameplayPlayed(true);
            _persistingService.SetCurrentShift(shiftIndex);
            _persistingService.SetCurrentCase(caseIndex);
            _persistingService.SetCasesOrder(Enumerable.Range(0, TotalCaseCountInCurrentShift).Shuffle().ToList());
            _persistingService.SetCaseResultHistory(new List<bool>());

            _onChangeShift.OnNext(Unit.Default);
            _onChangeCase.OnNext(Unit.Default);

            TryShowNewToolPopupForCurrentShift();
        }

        private void TryShowNewToolPopupForCurrentShift()
        {
            if (CurrentCaseIndex != 0)
                return;

            var shiftNumber = CurrentShiftIndex + 1;
            INewToolPopupPresenter presenter = null;
            if (shiftNumber == _gameplayConfig.LinkToolOpenShift)
                presenter = new LinkTool_NewToolPopupPresenter();
            else if (shiftNumber == _gameplayConfig.SenderToolOpenShift)
                presenter = new ContactTool_NewToolPopupPresenter();
            else if (shiftNumber == _gameplayConfig.DbToolOpenShift)
                presenter = new DbTool_NewToolPopupPresenter();

            if (presenter == null)
                return;

            _popupService.OpenPopup<INewToolPopupPresenter>(presenter, queued: true).Forget();
        }

        public async void MakeChoice(bool isValid)
        {
            if (!HelpWasUsedInCurrentCase)
            {
                _popupService.OpenPopup<ICaseResultPopupPresenter>(new Help_CaseResultPopupPresenter(isValid == GetExpectedDecisionForCase(CurrentCaseIndex), CurrentCaseConfig.ResultText)).Forget();
                await UniTask.WaitWhile(() => _popupService.IsPopupOpened<ICaseResultPopupPresenter>());
            }

            _persistingService.SetHelpWasUsedInCurrentCase(false);

            var nextCaseIndex = CurrentCaseIndex + 1;
            if (nextCaseIndex < _gameplayConfig.Shifts[CurrentShiftIndex].Cases.Count)
            {
                _persistingService.SetCaseResultHistory(_persistingService.CaseDecisionHistory.Append(isValid).ToList());
                CurrentCaseIndex = nextCaseIndex;
                _onChangeCase.OnNext(Unit.Default);
                return;
            }

            ShowShiftResultsPopup(isValid);
        }

        private void ShowShiftResultsPopup(bool lastCaseIsValid)
        {
            var allDecisions = _persistingService.CaseDecisionHistory.Append(lastCaseIsValid).ToList();
            var checkedAmount = allDecisions.Count;
            var correctCount = allDecisions.Select((d, i) => d == GetExpectedDecisionForCase(i)).Count(x => x);
            var errorsAmount = checkedAmount - correctCount;
            var winrate = (int)(correctCount / (float)checkedAmount * 100);

            var isSuccess = winrate >= _gameplayConfig.Shifts[CurrentShiftIndex].MinWinrate;

            IShiftResultsPopupPresenter presenter = isSuccess
                ? new Success_ShiftResultsPopupPresenter(checkedAmount, errorsAmount, winrate, OnShiftResultClick)
                : (IShiftResultsPopupPresenter)new Fail_ShiftResultsPopupPresenter(checkedAmount, errorsAmount, winrate, OnShiftResultClick);

            _popupService.OpenPopup<IShiftResultsPopupPresenter>(presenter, queued: true).Forget();

            void OnShiftResultClick(IShiftResultsPopupPresenter.ClickType clickType)
            {
                switch (clickType)
                {
                    case IShiftResultsPopupPresenter.ClickType.Continue:
                        var nextShiftIndex = CurrentShiftIndex + 1;
                        if (nextShiftIndex < _gameplayConfig.Shifts.Count)
                            ForceProgressTo(nextShiftIndex, 0);
                        else
                        {
                            _persistingService.ResetGameplayProgress();
                            _scenesService.LaunchMainMenu();
                            _popupService.OpenPopup<IGameWinPopupPresenter>(new GameWinPopupPresenter(), queued: true, withBackGround: false).Forget();
                        }
                        break;
                    case IShiftResultsPopupPresenter.ClickType.Retry:
                        ForceProgressTo(CurrentShiftIndex, 0);
                        break;
                    case IShiftResultsPopupPresenter.ClickType.Menu:
                        _scenesService.LaunchMainMenu();
                        break;
                }
            }
        }

        public void TryUseHelp()
        {
            if (AvailableHelpsNumber > 0)
            {
                _persistingService.SetAvailableHelpsNumber(AvailableHelpsNumber - 1);
                _persistingService.SetHelpWasUsedInCurrentCase(true);
                _onUseHelp.OnNext(Unit.Default);
                _popupService.OpenPopup<ICaseResultPopupPresenter>(new Help_CaseResultPopupPresenter(GetExpectedDecisionForCase(CurrentCaseIndex), CurrentCaseConfig.ResultText)).Forget();
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