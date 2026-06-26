using System;
using System.Collections;
using System.Collections.Generic;

namespace common.Utility
{
    /// <summary>
    /// Словарь с порядком согласным добавлению записей
    /// </summary>
    public class OrderedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly List<TKey> _keys;

        public int Count => _dictionary.Count;

        public OrderedDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _keys = new List<TKey>();
        }

        public OrderedDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
            _keys = new List<TKey>(capacity);
        }

        // Добавление нового элемента
        public void Add(TKey key, TValue value)
        {
            if (_dictionary.ContainsKey(key))
                throw new ArgumentException($"Key '{key}' already exists");

            _dictionary.Add(key, value);
            _keys.Add(key);
        }

        // Удаление элемента
        public bool Remove(TKey key)
        {
            if (_dictionary.Remove(key))
            {
                _keys.Remove(key);
                return true;
            }

            return false;
        }

        // Обновление/добавление через индексатор
        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                if (!_dictionary.ContainsKey(key))
                    _keys.Add(key);

                _dictionary[key] = value;
            }
        }

        // Проверка наличия ключа
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        // Попытка получить значение
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        // Очистка всех элементов
        public void Clear()
        {
            _dictionary.Clear();
            _keys.Clear();
        }

        // Получение ключа по индексу
        public TKey GetKeyAt(int index)
        {
            return _keys[index];
        }

        // Получение значения по индексу
        public TValue GetValueAt(int index)
        {
            return _dictionary[_keys[index]];
        }

        // Перебор в порядке вставки
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var key in _keys)
            {
                yield return new KeyValuePair<TKey, TValue>(key, _dictionary[key]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Получение всех ключей в порядке вставки
        public IEnumerable<TKey> Keys => _keys;

        // Получение всех значений в порядке вставки
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var key in _keys)
                    yield return _dictionary[key];
            }
        }
    }
}