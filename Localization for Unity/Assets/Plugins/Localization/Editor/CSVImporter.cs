using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Localization
{
    public class CSVImporter
    {
        private char Separator => CSVExporter.Separator;

        [MenuItem("YourGameName/Localization/Import strings")]
        public static void ImportFromMenu()
        {
            new CSVImporter().Import();
        }

        private void Import()
        {
            string csv = LoadCsv(CSVExporter.FilePath);
            List<string> blocks = ReadAllBlocks(csv);
            
            List<LocalizedString> strings = Resources.LoadAll<LocalizedString>(string.Empty).ToList();
            Debug.Log($"LocalizedString amount: {strings.Count}");
            
            foreach (List<string> line in ReadLines(blocks, columns: 3).Skip(1))
            {
                string id = line[0];
                string english = line[1];
                string russian = line[2];

                LocalizedString locStr = strings.SingleOrDefault(_ => _.Id == id);
                if (locStr == null)
                {
                    Debug.Log($"Can't find loc-string with id: '{id}'");
                    continue;
                }

                if (locStr.English != english)
                {
                    Debug.Log($"Update-en: '{locStr.Id}': from '{locStr.English}' to '{english}'");
                    locStr.English = english;
                    EditorUtility.SetDirty(locStr);
                }

                if (locStr.Russian != russian)
                {
                    Debug.Log($"Update-ru: '{locStr.Id}': from '{locStr.Russian}' to '{russian}'");
                    locStr.Russian = russian;
                    EditorUtility.SetDirty(locStr);
                }

                strings.Remove(locStr);
            }

            if (strings.Any())
            {
                Debug.Log("Missed localization for following loc-strings:");
                foreach(LocalizedString locStr in strings)
                    Debug.Log(locStr.Id);
            }

            Debug.Log("Import done!");
        }

        private string LoadCsv(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception($"Can't find csv file by path: '{filePath}'");
            
            return File.ReadAllText(filePath);
        }

        private List<string> ReadAllBlocks(string csv)
        {
            List<string> blocks = new List<string>();
            string block = string.Empty;
            bool isInsideQuotes = false;
            for (int i = 0; i < csv.Length; i++)
            {
                if (IsSeparator(csv[i]) && !isInsideQuotes)
                {
                    if (!string.IsNullOrEmpty(block))
                        blocks.Add(block);
                    block = string.Empty;
                }
                else if (csv[i] == '"')
                {
                    isInsideQuotes = !isInsideQuotes;
                }
                else
                {
                    block += csv[i];
                    if (i == csv.Length - 1)
                        blocks.Add(block);
                }
            }
            return blocks;
        }

        private bool IsSeparator(char ch) => ch == Separator || ch == '\n' || ch == '\r';

        private IEnumerable<List<string>> ReadLines(List<string> blocks, int columns)
        {
            List<string> line = new List<string>();
            foreach (string block in blocks)
            {
                line.Add(block);
                if (line.Count == columns)
                {
                    yield return line;
                    line = new List<string>();
                }
            }
        }
    }
}