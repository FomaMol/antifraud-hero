using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions.Collections;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace Common.Extensions
{
	public static class EnumerableExtension
	{
		[ContractAnnotation("collection:null => false")]
		public static bool IsFilled(this IEnumerable collection)
		{
			return collection != null && collection.GetEnumerator().MoveNext();
		}

		[ContractAnnotation("collection:null => true")]
		public static bool IsEmpty(this IEnumerable collection)
		{
			return collection == null || !collection.GetEnumerator().MoveNext();
		}

		public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> enumeration) => enumeration.Select((item, index) => (item, index));

		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumeration) => enumeration.Where(i => i != null);

		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (var element in enumeration)
				action(element);
		}
		
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
		{
			var index = 0;
			foreach (var element in enumeration)
				action(element, index++);
		}

		public static void ForEach<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumeration, Action<TKey, TValue> action)
		{
			foreach (var kv in enumeration)
				action(kv.Key, kv.Value);
		}
		
		public static void ForEach<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumeration, Action<TKey, TValue, int> action)
		{
			var index = 0;
			foreach (var kv in enumeration)
				action(kv.Key, kv.Value, index++);
		}

		public static bool NoneOf<T>(this IEnumerable<T> enumeration, Func<T, bool> action)
		{
			return !enumeration.Any(action);
		}

		/// Проходит по всей коллекции (без прерывания, в отличии от All()); возвращает true, если все элементы true
		public static bool ForEachSuccess(this IEnumerable<bool> enumeration)
		{
			return enumeration.Aggregate(true, (current, element) => current & element);
		}

		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
		{
			foreach (var element in range)
				hashSet.Add(element);
		}

		public static bool EqualTo<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2)
		{
			if (seq1 == null && seq2 == null)
				return true;

			if (seq1 == null || seq2 == null)
				return false;

			return seq1.SequenceEqual(seq2);
		}

		public static Dictionary<TValue, int> GetCountOf<TEnum, TValue>(this IEnumerable<TEnum> enumerable, Func<TEnum, TValue> selector)
		{
			var itemsCount = new Dictionary<TValue, int>();
			foreach (var item in enumerable.Select(selector).NotNull())
				itemsCount.Append(item, 1);
			return itemsCount;
		}

		public static int Count(this IEnumerable e)
		{
			if (e == null)
				return 0;

			var c = 0;
			var n = e.GetEnumerator();
			while (n.MoveNext())
				c++;

			return c;
		}

		/// <summary>
		/// Если перечисление не нул и содержит элементы - возвращает Max(), иначе дефолтное значение
		/// </summary>
		public static T MaxOrDefault<T>(this IEnumerable<T> seq, T def = default)
		{
			return seq.IsEmpty() ? def : seq.Max();
		}

		public static T MinOrDefault<T>(this IEnumerable<T> seq, Func<T, T, int> compareTo, T def = default)
		{
			return seq.IsEmpty() ? def : seq.Aggregate((min, x) => (min == null || compareTo(min, x) > 0 ? x : min));
		}

		public static T MaxOrDefault<T>(this IEnumerable<T> seq, Func<T, T, int> compareTo, T def = default)
		{
			return seq.IsEmpty() ? def : seq.Aggregate((max, x) => (max == null || compareTo(max, x) < 0 ? x : max));
		}

		public static void Deconstruct<T1, T2>(this IGrouping<T1, T2> groups, out T1 key, out IEnumerable<T2> values)
		{
			key = groups.Key;
			values = groups;
		}

		public static string JoinToString(this IEnumerable seq, string delimiter = null)
		{
			if (seq == null)
				return "null";

			var sb = new StringBuilder();

			var n = seq.GetEnumerator();
			var i = 0;
			while (n.MoveNext())
			{
				if (i > 0 && delimiter.IsFilled())
					sb.Append(delimiter);

				var value = n.Current;
				if (value == null)
					sb.Append("null");
				else
					sb.Append(n.Current);

				i++;
			}

			return sb.ToString();
		}


		public static string ToDebugString(this IEnumerable seq)
		{
			if (seq == null)
				return "null";

			return "[" + seq.JoinToString(", ") + "]";
		}

		public static int IndexOf<T>(this IEnumerable<T> enumerable, T element, IEqualityComparer<T> comparer = null)
		{
			int i = 0;
			comparer ??= EqualityComparer<T>.Default;
			foreach (var currentElement in enumerable)
			{
				if (comparer.Equals(currentElement, element))
					return i;
				i++;
			}
			return -1;
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			IList<T> list = source.ToList();
			list.Shuffle();
			return list;
		}

		public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> enumerable, int numberOfElements)
		{
			var list = enumerable.ToList();
			if (list.Count == 0)
				return Enumerable.Empty<T>();

			if (numberOfElements > list.Count)
				numberOfElements = list.Count;

			var randomElements = new List<T>();
			for (var i = 0; i < numberOfElements; i++)
			{
				var randomIndex = Random.Range(0, list.Count);
				randomElements.Add(list[randomIndex]);
				list.RemoveAt(randomIndex);
			}

			return randomElements;
		}
		
		public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, IEnumerable<T> append)
		{
			foreach (T obj in source)
				yield return obj;
			foreach (T obj in append)
				yield return obj;
		}
	}
}