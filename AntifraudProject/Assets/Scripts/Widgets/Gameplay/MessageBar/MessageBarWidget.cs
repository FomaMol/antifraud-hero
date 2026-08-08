using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Localization;
using Services;
using UniRx;
using UnityEngine;
using VContainer;

namespace Widgets
{
    public class MessageBarWidget : Widget
    {
        [Inject] private readonly GameplayService _gameplayService;
        [Inject] private readonly LocalizationService _localizationService;
        
        [SerializeField] private TMPLinkHandler _tmpLinkHandler;
        [SerializeField] private PeaceOfTextWidget _linkTextWidget;
        [SerializeField] private PeaceOfTextWidget _contactTextWidget;
        [SerializeField] private PeaceOfTextWidget _dbTextWidget;
        [Space]
        [SerializeField] private ToolsBarWidget _toolsBarWidget; // TODO: Избавиться от прямой ссылки

        public void Initialize()
        {
            _tmpLinkHandler.OnLinkClicked += TmpLinkHandlerOnOnLinkClicked;
            _gameplayService.OnChangeCase.Subscribe(OnChangeCase);
            
            UpdateView();
        }

        private static readonly string[] CustomTagTypes = { "link", "contact", "db" };

        public static (string text, IReadOnlyDictionary<string, string> tagContents) ReplaceCustomTags(string text)
        {
            var tagContents = new Dictionary<string, string>();
            foreach (var type in CustomTagTypes)
            {
                text = Regex.Replace(text, $@"\[{type}\](.*?)\[\/{type}\]", match =>
                {
                    tagContents[type] = match.Groups[1].Value;
                    return $"<link={type}><color=#CEBA18>{match.Groups[1].Value}</color></link>";
                });
            }
            return (text, tagContents);
        }

        private void TmpLinkHandlerOnOnLinkClicked(string id, string linkText)
        {
            InsertDataIntoTool(id, linkText);
        }

        private void OnChangeCase(Unit unit)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var localizedMessage = _localizationService.Get(_gameplayService.CurrentCaseConfig.MessageText);
            var (processedText, tagContents) = ReplaceCustomTags(localizedMessage);
            _tmpLinkHandler.Text.SetText(processedText);
            
            _linkTextWidget.gameObject.SetActive(false);
            _contactTextWidget.gameObject.SetActive(false);
            _dbTextWidget.gameObject.SetActive(false);
            foreach (var tagContent in tagContents)
            {
                if (tagContent.Key == "link")
                {
                    _linkTextWidget.Initialize(tagContent.Key, tagContent.Value, OnDataButtonClick);
                    _linkTextWidget.gameObject.SetActive(true);
                }
                if (tagContent.Key == "contact")
                {
                    _contactTextWidget.Initialize(tagContent.Key, tagContent.Value, OnDataButtonClick);
                    _contactTextWidget.gameObject.SetActive(true);
                }
                if (tagContent.Key == "db")
                {
                    _dbTextWidget.Initialize(tagContent.Key, tagContent.Value, OnDataButtonClick);
                    _dbTextWidget.gameObject.SetActive(true);
                }
            }
        }

        private void OnDataButtonClick((string dataType, string text) data)
        {
            InsertDataIntoTool(data.dataType, data.text);
        }

        private void InsertDataIntoTool(string dataType, string text)
        {
            Debug.Log($"Type: {dataType} Text: {text}");
            if (dataType == "link") // TODO: что-то сделать с типами
            {
                _toolsBarWidget.InsertData(ToolType.Link, text);
            }
            else if (dataType == "contact")
            {
                _toolsBarWidget.InsertData(ToolType.Contact, text);
            }
            else if (dataType == "db")
            {
                _toolsBarWidget.InsertData(ToolType.Db, text);
            }
        }

        private void Start()
        {
            Initialize();
        }
    }
}