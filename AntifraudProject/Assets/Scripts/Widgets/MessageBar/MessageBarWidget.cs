using System;
using System.Text.RegularExpressions;
using Services;
using UniRx;
using UnityEngine;
using VContainer;

namespace Widgets
{
    public class MessageBarWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
        
        [SerializeField] private TMPLinkHandler _tmpLinkHandler;

        public void Initialize()
        {
            _tmpLinkHandler.OnLinkClicked += TmpLinkHandlerOnOnLinkClicked;
            _gameplayService.OnChangeCase.Subscribe(OnChangeCase);
            
            UpdateView();
        }

        private static readonly string[] CustomTagTypes = { "link", "contact", "db" };

        public static string ReplaceCustomTags(string text)
        {
            foreach (var type in CustomTagTypes)
            {
                text = Regex.Replace(text, $@"\[{type}\](.*?)\[\/{type}\]", $"<link={type}><u><color=#4A90D9>$1</color></u></link>");
            }
            return text;
        }

        private void TmpLinkHandlerOnOnLinkClicked(string id, string linkText)
        {
            Debug.Log($"Click on link: id={id}, text={linkText}");
        }

        private void OnChangeCase(Unit unit)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _tmpLinkHandler.Text.SetText(ReplaceCustomTags(_gameplayService.CurrentCaseConfig.MessageText));
        }

        private void Start()
        {
            Initialize();
        }
    }
}