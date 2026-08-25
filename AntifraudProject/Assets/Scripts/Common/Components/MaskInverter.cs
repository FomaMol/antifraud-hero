using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Common.Components
{
    [AddComponentMenu("DreamCode/UI/Mask Inverter")]
    [ExecuteAlways]
    public sealed class MaskInverter : MonoBehaviour, IMaterialModifier
    {
        private static readonly int _stencilComp = Shader.PropertyToID("_StencilComp");

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            var resultMaterial = new Material(baseMaterial);
            resultMaterial.SetFloat(_stencilComp, Convert.ToSingle(CompareFunction.NotEqual));
            return resultMaterial;
        }

        private void Start()
        {
            StartCoroutine(ToggleMaskable());
        }

        private IEnumerator ToggleMaskable()
        {
            yield return null;
            var maskableGraphic = GetComponent<MaskableGraphic>();
            maskableGraphic.maskable = false;
            maskableGraphic.maskable = true;
        }
    }
}