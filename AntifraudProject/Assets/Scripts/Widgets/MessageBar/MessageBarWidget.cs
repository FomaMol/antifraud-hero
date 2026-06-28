using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Widgets
{
    public class MessageBarWidget : Widget
    {
        [SerializeField] private TMPLinkHandler _tmpLinkHandler;

        public void Initialize()
        {
            _tmpLinkHandler.OnLinkClicked += TmpLinkHandlerOnOnLinkClicked;
            _tmpLinkHandler.Text.SetText(ReplaceCustomTags("Sended [contact]Nadya[/contact]. Ваша карта заблокирована ... нажмите на ссылка [link]https://vk.com[/link]"));
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

        private void Start()
        {
            Initialize();
        }
    }
}