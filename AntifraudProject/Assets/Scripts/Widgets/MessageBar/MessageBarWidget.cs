using System;
using System.Collections.Generic;
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
        [SerializeField] private PeaceOfTextWidget _linkTextWidget;
        [SerializeField] private PeaceOfTextWidget _contactTextWidget;
        [SerializeField] private PeaceOfTextWidget _dbTextWidget;

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
                    return $"<link={type}><u><color=#4A90D9>{match.Groups[1].Value}</color></u></link>";
                });
            }
            return (text, tagContents);
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
            var (processedText, tagContents) = ReplaceCustomTags(_gameplayService.CurrentCaseConfig.MessageText);
            _tmpLinkHandler.Text.SetText(processedText);
            
            _linkTextWidget.gameObject.SetActive(false);
            _contactTextWidget.gameObject.SetActive(false);
            _dbTextWidget.gameObject.SetActive(false);
            foreach (var tagContent in tagContents)
            {
                if (tagContent.Key == "link")
                {
                    _linkTextWidget.Initialize(tagContent.Key, tagContent.Value, () => Debug.Log($"Type: {tagContent.Key} Text: {tagContent.Value}"));
                    _linkTextWidget.gameObject.SetActive(true);
                }
                if (tagContent.Key == "contact")
                {
                    _contactTextWidget.Initialize(tagContent.Key, tagContent.Value, () => Debug.Log($"Type: {tagContent.Key} Text: {tagContent.Value}"));
                    _contactTextWidget.gameObject.SetActive(true);
                }
                if (tagContent.Key == "db")
                {
                    _dbTextWidget.Initialize(tagContent.Key, tagContent.Value, () => Debug.Log($"Type: {tagContent.Key} Text: {tagContent.Value}"));
                    _dbTextWidget.gameObject.SetActive(true);
                }
            }
        }

        private void Start()
        {
            Initialize();
        }
    }
}