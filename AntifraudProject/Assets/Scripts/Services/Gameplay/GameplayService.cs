using Newtonsoft.Json;
using UnityEngine;
using VContainer.Unity;

namespace Services
{
    public class GameplayService : IInitializable
    {
        private GameplayConfig _gameplayConfig;

        public void Initialize()
        {
            var json = Resources.Load<TextAsset>("GameplayConfig").text;
            _gameplayConfig = JsonConvert.DeserializeObject<GameplayConfig>(json);  
        }
    }
}