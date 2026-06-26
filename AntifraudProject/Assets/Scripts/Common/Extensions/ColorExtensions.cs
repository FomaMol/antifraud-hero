using UnityEngine;

namespace Common.Extensions
{
	public static class ColorExtensions
	{
		public static Color WithAlpha(this Color color, float alpha) {
			return new Color(color.r, color.g, color.b, alpha);
		}
		
		public static string ToRGBHex(this Color c)
		{
			return c.ToInt().ToString("X");
		}

		public static string ToARGBHex(this Color c)
		{
			return c.ToIntWithAlpha().ToString("X");
		}

		private static byte ToByte(float f)
		{
			f = Mathf.Clamp01(f);
			return (byte)(f * 255);
		}

		public static int ToInt(this Color color)
		{
			var r = Mathf.RoundToInt(color.r * 255f);
			var g = Mathf.RoundToInt(color.g * 255f);
			var b = Mathf.RoundToInt(color.b * 255f);

			var intValue = (r << 16) | (g << 8) | b;
			return intValue;
		}

		public static int ToIntWithAlpha(this Color color)
		{
			var r = Mathf.RoundToInt(color.r * 255f);
			var g = Mathf.RoundToInt(color.g * 255f);
			var b = Mathf.RoundToInt(color.b * 255f);
			var a = Mathf.RoundToInt(color.a * 255f);

			var intValue = (a << 24) | (r << 16) | (g << 8) | b;
			return intValue;
		}
	}
}