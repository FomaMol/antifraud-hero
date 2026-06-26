using UnityEngine;

namespace Common.Extensions
{
	public static class IntExtensions
	{
		public static bool IsEven(this int value) => value % 2 == 0;

		public static bool IsOdd(this int value) => !IsEven(value);

		public static Color ToColor(this int value)
		{
			return new Color32((byte)(value >> 16), (byte)(value >> 8), (byte)value, 255);
		}

		public static Color ToColorWithAlpha(this int value)
		{
			return new Color32((byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value);
		}

		public static string GetCompactCount(this int value)
		{
			return value switch
			{
				< 10000 => value.ToString(),
				< 1000000 => value / 1000 + "K",
				< 1000000000 => value / 1000000 + "M",
				_ => value / 1000000000 + "B"
			};
		}
	}
}