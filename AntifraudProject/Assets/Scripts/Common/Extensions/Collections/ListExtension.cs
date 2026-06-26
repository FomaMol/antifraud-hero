using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace Common.Extensions
{
	public static class ListExtension
	{
		public static bool ForEachAny<T>(this IReadOnlyList<T> list, Func<T, bool> action)
		{
			return list != null && list.Aggregate(false, (current, t) => current | action(t));
		}

		[ContractAnnotation("list:null => false")]
		public static bool IsFilled<T>(this List<T> list)
		{
			return list != null && list.Count > 0;
		}

		[ContractAnnotation("list:null => true")]
		public static bool IsEmpty<T>(this List<T> list)
		{
			return list == null || list.Count == 0;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			if (list == null)
				return;

			var random = new System.Random();

			for (var i = 0; i < list.Count; i++)
			{
				var randomIndex = random.Next(i, list.Count);
				(list[i], list[randomIndex]) = (list[randomIndex], list[i]);
			}
		}

		public static T GetRandom<T>(this IReadOnlyList<T> list)
		{
			T wanted = default;
			if (list == null)
				return wanted;

			var count = list.Count;
			if (count == 0)
				return wanted;

			var i = Random.Range(0, count);
			return list[i];
		}

		public static void AddIfNewAndNotNull<T>(this IList<T> list, T value)
		{
			if (!typeof(T).IsValueType && Equals(value, null))
				return;

			if (list == null)
				return;

			if (!list.Contains(value))
				list.Add(value);
		}

		public static void AddRangeNewNotNull<T>(this IList<T> list, IEnumerable<T> collection)
		{
			if (list == null)
				return;

			foreach (var element in collection)
				list.AddIfNewAndNotNull(element);
		}

		public static List<T> CloneList<T>(this IList<T> list) where T : ICloneable
		{
			return list?.Select(item => item == null ? default : (T)item.Clone()).ToList();
		}

		public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (var i = 0; i < list.Count; i++)
			{
				if (!predicate(list[i]))
					continue;

				list.RemoveAt(i);
				i--;
			}
		}

		public static IEnumerable<T> GetAndRemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (var i = 0; i < list.Count; i++)
			{
				var item = list[i];
				if (!predicate(item))
					continue;

				list.RemoveAt(i);
				i--;
				yield return item;
			}
		}

		public static T GetAndRemove<T>(this IList<T> list, int index)
		{
			var value = list[index];
			list.RemoveAt(index);
			return value;
		}

		public static void MoveToEnd<T>(this IList<T> list, int index)
		{
			var value = list.GetAndRemove(index);
			list.Add(value);
		}

		public static bool IndexOutOfBounds<T>(this IReadOnlyList<T> list, int index)
		{
			return list == null || index < 0 || index > list.Count - 1;
		}

		public static bool IsIndexInBounds<T>(this IReadOnlyList<T> list, int index)
		{
			return !list.IndexOutOfBounds(index);
		}

		public static bool UnorderedEqualTo<T>(this IReadOnlyList<T> seq1, IReadOnlyList<T> seq2)
		{
			if (seq1 == null && seq2 == null)
				return true;

			if (seq1 == null || seq2 == null)
				return false;

			return seq1.Count == seq2.Count && seq1.All(seq2.Contains);
		}

		public static T GetAtOrDefault<T>(this IReadOnlyList<T> list, int index)
		{
			if (list == null || index >= list.Count || index < 0)
				return default;

			return list[index];
		}

		public static IEnumerable<T> GetSafeRange<T>(this IReadOnlyList<T> list, int index, int count)
		{
			if (list.IndexOutOfBounds(index))
				yield break;

			var indexMax = Math.Min(list.Count, index + count);
			for (var i = index; i < indexMax; i++)
				yield return list[i];
		}

		public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list.Count == 0;
	}
}