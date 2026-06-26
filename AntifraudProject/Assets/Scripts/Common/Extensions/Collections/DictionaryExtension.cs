using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Common.Extensions.Collections
{
	public static class DictionaryExtension
	{
		public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
		{
			if (dic == null || key == null)
				return;

			dic[key] = value;
		}

		public static void Append<TKey>(this IDictionary<TKey, int> dict, TKey key, int value)
		{
			if (dict.ContainsKey(key))
				dict[key] += value;
			else
				dict.Add(key, value);
		}

		public static void SetValues<TKey, TValue>(this IDictionary<TKey, TValue> dic, IDictionary<TKey, TValue> values)
		{
			if (dic == null || values == null)
				return;

			foreach (var entry in values)
				dic.SetValue(entry.Key, entry.Value);
		}

		public static TKey GetValueKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue searchValue)
		{
			foreach (var (key, value) in dict)
			{
				if (value.Equals(searchValue))
					return key;
			}
			return default;
		}

		public static bool EqualTo<TKey, TValue>(this IDictionary<TKey, TValue> dic, IDictionary<TKey, TValue> compareDictionary)
		{
			if (dic == compareDictionary)
				return true;

			if (dic == null || compareDictionary == null)
				return false;

			if (dic.Count != compareDictionary.Count)
				return false;

			foreach (var pair in compareDictionary)
			{
				if (!dic.ContainsKey(pair.Key))
					return false;

				if (!pair.Value.Equals(dic[pair.Key]))
					return false;
			}

			return true;
		}

		public static void RemoveWhere<TKey, TValue>(this IDictionary<TKey, TValue> dict, Func<TKey, TValue, bool> predicate)
		{
			var keysToRemove = dict.Keys
				.Where(k => predicate(k, dict[k]))
				.ToHashSet();

			EnumerableExtension.ForEach(keysToRemove, k => dict.Remove(k));
		}

		// слияние двух словарей формата предмет-количество
		public static void Merge<TKey>(this Dictionary<TKey, int> dict, Dictionary<TKey, int> other)
		{
			var keys = new HashSet<TKey>(other.Keys);
			foreach (var key in keys)
				dict.Append(key, other.GetValueOrDefault(key));
		}

		public static T GetClosestValueOrDefault<T>(this SortedDictionary<int, T> dict, int key, T defaultValue = default)
		{
			var keys = dict.Keys.ToArray();
			var keyIndex = keys.FindClosestIndex(key);

			return keys.IndexOutOfBounds(keyIndex) ? defaultValue : dict[keys[keyIndex]];
		}

		public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
		{
			key = tuple.Key;
			value = tuple.Value;
		}

		public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			const string separator = ",\n"; // почему-то генерик не распознаётся с аргументом по умолчанию

			if (dictionary == null)
				return "null";

			return "{" + string.Join(separator, dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
		}

		public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
		{
			if (!dictionary.ContainsKey(key))
				dictionary.Add(key, new TValue());

			return dictionary[key];
		}
		
		public static Dictionary<T1, T2> UnionWith<T1, T2>(this Dictionary<T1, T2> dict1, Dictionary<T1, T2> dict2)
		{
			return dict1.Union(dict2).ToDictionary(it => it.Key, it => it.Value);
		}
        
		public static string ConvertToJson(this Dictionary<string, object> parameters)
		{
			return JsonConvert.SerializeObject(parameters, Formatting.Indented);
		}
	}
}