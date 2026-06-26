using System;
using JetBrains.Annotations;

namespace Common.Extensions
{
	public static class ArrayExtension
	{
		[ContractAnnotation("array:null => false")]
		public static bool IsFilled<T>(this T[] array)
		{
			return array != null && array.Length > 0;
		}

		[ContractAnnotation("array:null => true")]
		public static bool IsEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}

		public static T[] Combine<T>(this T[] arr, params T[] inclusion)
		{
			if (arr == null)
				return Array.Empty<T>();

			var arrLength = arr.Length;
			var inclusionLength = inclusion?.Length ?? 0;

			var combinedArr = new T[arrLength + inclusionLength];

			Array.Copy(arr, combinedArr, arrLength);

			if (inclusionLength != 0)
				Array.Copy(inclusion, 0, combinedArr, arrLength, inclusionLength);

			return combinedArr;
		}

		public static T[] InsertAt<T>(this T[] origin, T value, int index)
		{
			var newSize = origin.Length + 1;
			var result = new T[newSize];

			for (var i = 0; i < newSize; i++)
			{
				if (i < index)
					result[i] = origin[i];
				else if (i > index)
					result[i] = origin[i - 1];
				else
					result[i] = value;
			}

			return result;
		}

		public static T[] RemoveAt<T>(this T[] origin, int index)
		{
			var newSize = origin.Length - 1;
			var result = new T[newSize];

			for (var i = 0; i < newSize; i++)
			{
				if (i < index)
					result[i] = origin[i];
				else if (i >= index)
					result[i] = origin[i + 1];
			}

			return result;
		}

		/// <summary> Производит бинарный поиск по отсортированному массиву и возвращает ближайший сверху индекс с заданным значением (весом) </summary>
		public static int FindClosestIndex<T>(this T[] array, T value)
		{
			var index = Array.BinarySearch(array, value);
			return index < 0 ? ~index : index;
		}
	}
}