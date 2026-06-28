using System;
using System.Collections.Generic;
using DI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Widgets;

namespace Localization
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : MonoBehaviour
    {
        [Inject] private LocalizationService _localization;

        [SerializeField] private List<LanguageImagePair> _images;

        private Image Image => _image ??= GetComponent<Image>();
        private Image _image;

        private void Start()
        {
            _localization.OnLanguageChanged += ChangeImage;
            ChangeImage();
        }

        private void ChangeImage()
        {
            var pair = _images.Find(pair => pair.Language == _localization.Language);
            if (pair != null)
            {
                Image.sprite = pair.Image;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _localization.OnLanguageChanged -= ChangeImage;
        }

    }

    [Serializable]
    public class LanguageImagePair
    {
        public string Language;
        public Sprite Image;
    }
}