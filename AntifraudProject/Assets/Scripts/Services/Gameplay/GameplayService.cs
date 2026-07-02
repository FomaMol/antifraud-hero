using System;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace Services
{
    public class GameplayService : IInitializable
    {
        private GameplayConfig _gameplayConfig;
        
        public int CurrentShiftIndex { get; private set; }
        public int CurrentCaseIndex { get; private set; }
        public CaseConfig CurrentCaseConfig => _gameplayConfig.Shifts[CurrentShiftIndex].Cases[CurrentCaseIndex];

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
            }
            if (CurrentShiftIndex >= _gameplayConfig.Shifts.Count)
            {
                CurrentShiftIndex = 0;
            }

            _onChangeCase.OnNext(Unit.Default);
        }
    }
}