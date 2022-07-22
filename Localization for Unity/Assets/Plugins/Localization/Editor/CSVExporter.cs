using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Localization
{
	public class CSVExporter
	{
		public const char Separator = ',';
		public const string FilePath = "Assets/strings.csv";

		[MenuItem("YourGameName/Localization/Export strings")]
		public static void Export()
		{
			Debug.Log("Start export localization strings to CSV.");

			LocalizedString[] strings = Resources.LoadAll<LocalizedString>(string.Empty);
			Debug.Log($"{strings.Length} strings are loaded.");

			List<string> lines = new List<string>();
			lines.Add(GetHeader());
			lines.AddRange(strings.Select(GetString));

			File.WriteAllLines(FilePath, lines.ToArray());
			AssetDatabase.Refresh();
			Debug.Log(string.Format("Strings is saved by path: " + FilePath));
		}

		private static string GetHeader()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("ID");
			sb.Append(Separator);

			sb.Append("English");
			sb.Append(Separator);

			sb.Append("Russian");

			return sb.ToString();
		}

		private static string GetString(LocalizedString localizedString)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(localizedString.Id);
			sb.Append(Separator);

			sb.Append($"\"{localizedString.English}\"");
			sb.Append(Separator);

			sb.Append($"\"{localizedString.Russian}\"");

			return sb.ToString();
		}
	}
}