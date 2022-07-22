using UnityEditor;
using UnityEngine;

namespace Localization
{
	[CustomPropertyDrawer(typeof(LocalizedString))]
	public class LocalizedStringPropertyDrawer : PropertyDrawer
	{
		private string _stringId;

		private float LineHeight => EditorGUIUtility.singleLineHeight;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return LineHeight * (property.objectReferenceValue == null ? 2 : 3);
		}

		public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
		{
			if (property.objectReferenceValue == null)
			{
				EditorGUI.PropertyField(GetLinePos(pos, 1), property);

				EditorGUI.indentLevel++;

				_stringId = EditorGUI.TextField(GetLinePos(pos, 2, 1, 2), _stringId);
				if (GUI.Button(GetLinePos(pos, 2, 2, 2), "Create"))
				{
					if (!string.IsNullOrEmpty(_stringId))
					{
						LocalizedString localizedString = CreateNewLocalizedString(_stringId);
						property.objectReferenceValue = localizedString;
					}
				}

				EditorGUI.indentLevel--;
			}
			else
			{
				EditorGUI.PropertyField(GetLinePos(pos, 1), property);

				LocalizedString localizedString = property.objectReferenceValue as LocalizedString;
				if (localizedString != null)
				{
					EditorGUI.indentLevel++;

					if (GUI.Button(GetLinePos(pos, 2, 1, 4), ""))
					{
						Debug.Log("En");
						LocalizationSystem.ChangeLanguageFromEditor(SystemLanguage.English);
					}

					if (GUI.Button(GetLinePos(pos, 3, 1, 4), ""))
					{
						Debug.Log("Ru");
						LocalizationSystem.ChangeLanguageFromEditor(SystemLanguage.Russian);
					}

					localizedString.English = EditorGUI.TextField(GetLinePos(pos, 2), "English", localizedString.English);
					localizedString.Russian = EditorGUI.TextField(GetLinePos(pos, 3), "Russian", localizedString.Russian);
					
					EditorGUI.indentLevel--;
					
					EditorUtility.SetDirty(localizedString);
				}
			}
		}

		private LocalizedString CreateNewLocalizedString(string id)
		{
			LocalizedString localizedString = ScriptableObject.CreateInstance<LocalizedString>();
			localizedString.Id = id;
			string filePath = $"Assets/Resources/Localization/{localizedString.Id}.asset";
			AssetDatabase.CreateAsset(localizedString, filePath);
			AssetDatabase.SaveAssets();
			return localizedString;
		}

		private Rect GetLinePos(Rect pos, int line)
		{
			return new Rect(pos.x, pos.y + LineHeight * (line - 1), pos.width, LineHeight);
		}

		private Rect GetLinePos(Rect pos, int line, int part, int parts)
		{
			return new Rect(pos.x + pos.width / parts * (part - 1), pos.y + LineHeight * (line - 1), pos.width / parts, LineHeight);
		}
	}
}