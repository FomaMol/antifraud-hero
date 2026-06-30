using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Csv;
using UnityEngine;

namespace Localization
{
    public class LocalizationConfig
    {
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Table { get; private set; }

        public LocalizationConfig()
        {
            LoadFromResources("Localization");
        }

        private void LoadFromResources(string resourcePath)
        {
            var textAsset = Resources.Load<TextAsset>(resourcePath);
            if (textAsset == null)
            {
                Debug.LogError($"Failed to load localization config from {resourcePath}");
                Table = new Dictionary<string, IReadOnlyDictionary<string, string>>();
                return;
            }

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            using (var reader = new CsvFileReader(stream))
            {
                var table = new List<List<string>>();
                reader.ReadAll(table);
                Table = ParseDictionaryTable(table);
            }
        }

        private IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> ParseDictionaryTable(List<List<string>> table)
        {
            var result = new Dictionary<string, IReadOnlyDictionary<string, string>>();

            if (table.Count < 2)
                return result;

            // First row contains headers (languages)
            var headers = table[0].Skip(1).ToList();

            // Remaining rows contain localization keys and values
            for (int rowIdx = 1; rowIdx < table.Count; rowIdx++)
            {
                var row = table[rowIdx];
                if (row.Count == 0 || string.IsNullOrEmpty(row[0]))
                    continue;

                var key = row[0];
                var values = new Dictionary<string, string>();

                for (int colIdx = 1; colIdx < row.Count && colIdx <= headers.Count; colIdx++)
                {
                    var language = headers[colIdx - 1];
                    var value = colIdx < row.Count ? row[colIdx] : string.Empty;
                    values[language] = value ?? string.Empty;
                }

                result[key] = values;
            }

            return result;
        }
    }
}