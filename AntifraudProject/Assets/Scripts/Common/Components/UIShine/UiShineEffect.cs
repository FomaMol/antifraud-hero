using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Common.Components
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class UiShineEffect : BaseMeshEffect
	{
#if UNITY_EDITOR
		[SerializeField]
		private bool _isPlayInEditor = true;
#endif

		[SerializeField]
		private bool _isPlayOnAwake = true;

		[SerializeField]
		private bool _isLooped = true;
		[SerializeField] [Range(0, 120)]
		private float _loopInterval = 2f;
		[SerializeField] [Range(0.01f, 10f)]
		private float _playDuration = 1f;

		[Header("Shader params")]
		[SerializeField] [Range(0, 1)]
		private float _location;
		[SerializeField] [Range(0, 1)]
		private float _width = 0.15f;
		[SerializeField] [Range(0.01f, 1)]
		private float _softness = 1f;
		[SerializeField] [Range(0, 1)]
		private float _brightness = 1f;
		[SerializeField] [Range(-180, 180)]
		private float _rotation = 180f;
		[SerializeField] [Range(0, 1)]
		private float _highlight = 0.5f;

		[SerializeField] private Material _effectMaterial;

		private const int PRECISION_1 = (1 << 6) - 1;
		private const int PRECISION_2 = (1 << 12) - 1;

		private bool _isPlaying;

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
				return;

			var rect = graphic.rectTransform.rect;

			var rad = _rotation * Mathf.Deg2Rad;
			var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
			dir.x *= rect.height / rect.width;
			dir = dir.normalized;

			var vertex = default(UIVertex);
			var localMatrix = new Matrix2x3(rect, dir.x, dir.y);

			for (var i = 0; i < vh.currentVertCount; i++)
			{
				vh.PopulateUIVertex(ref vertex, i);

				var normPos = localMatrix * vertex.position;

				vertex.uv1 = new Vector2(
					PackToFloat(Mathf.Clamp01(normPos.y), _softness, _width, _brightness),
					PackToFloat(_location, _highlight)
				);

				vh.SetUIVertex(vertex, i);
			}
		}

		public void Play(float duration = 1f, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
		{
			StopAllCoroutines();
			StartCoroutine(CoPlay(duration, updateMode));
		}

		public void Stop()
		{
			StopAllCoroutines();

			_location = 0f;
			SetDirty();
		}

		protected override void OnEnable()
		{
			graphic.material = _effectMaterial;
			base.OnEnable();
		}

		protected override void OnDisable()
		{
			graphic.material = null;
			base.OnDisable();
		}

		protected override void Start()
		{
			base.Start();

			if (!_isPlayOnAwake || _isPlaying)
				return;

			Play(_playDuration);
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();

			Stop();
			if (!_isPlayInEditor || !isActiveAndEnabled)
				return;

			Play(_playDuration);
		}
#endif

		private IEnumerator CoPlay(float duration, AnimatorUpdateMode updateMode)
		{
			_isPlaying = true;

			float time = 0;
			while (time < duration)
			{
				_location = time / duration;
				time += updateMode == AnimatorUpdateMode.UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
				SetDirty();
				yield return null;
			}

			if (_isLooped)
			{
				yield return new WaitForSeconds(_loopInterval);
				Play(duration, updateMode);
			}

			_isPlaying = false;
		}

		private void SetDirty()
		{
			if (graphic)
				graphic.SetVerticesDirty();
		}

		/// <summary>
		/// Pack 4 low-precision [0-1] floats values to a float.
		/// Each value [0-1] has 64 steps(6 bits).
		/// </summary>
		private static float PackToFloat(float x, float y, float z, float w)
		{
			return (Mathf.FloorToInt(w * PRECISION_1) << 18)
				+ (Mathf.FloorToInt(z * PRECISION_1) << 12)
				+ (Mathf.FloorToInt(y * PRECISION_1) << 6)
				+ Mathf.FloorToInt(x * PRECISION_1);
		}

		/// <summary>
		/// Pack 2 low-precision [0-1] floats values to a float.
		/// Each value [0-1] has 4096 steps(12 bits).
		/// </summary>
		private static float PackToFloat(float x, float y)
		{
			return (Mathf.FloorToInt(y * PRECISION_2) << 12)
				+ Mathf.FloorToInt(x * PRECISION_2);
		}

		private struct Matrix2x3
		{
			public float m00, m01, m02, m10, m11, m12;

			public Matrix2x3(Rect rect, float cos, float sin)
			{
				const float center = 0.5f;
				var dx = -rect.xMin / rect.width - center;
				var dy = -rect.yMin / rect.height - center;
				m00 = cos / rect.width;
				m01 = -sin / rect.height;
				m02 = dx * cos - dy * sin + center;
				m10 = sin / rect.width;
				m11 = cos / rect.height;
				m12 = dx * sin + dy * cos + center;
			}

			public static Vector2 operator *(Matrix2x3 m, Vector2 v)
			{
				return new Vector2(
					(m.m00 * v.x) + (m.m01 * v.y) + m.m02,
					(m.m10 * v.x) + (m.m11 * v.y) + m.m12
				);
			}
		}
	}
}