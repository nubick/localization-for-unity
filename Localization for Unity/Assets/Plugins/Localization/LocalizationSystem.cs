using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Localization
{
	public class LocalizationSystem
	{
		public SystemLanguage Language { get; private set; }
		public List<ILocalized> Handlers { get; }
		public Dictionary<string, LocalizedString> StringsMap { get; private set; }

		public LocalizationSystem(List<ILocalized> handlers)
		{
			LoadStrings();
			Handlers = handlers;
			Debug.Log($"Loc: Register handlers, amount: {handlers.Count}");
			InitializeUsingSystemLanguage();
		}
		
		private void InitializeUsingSystemLanguage()
		{
			Debug.Log($"Loc: Initialize using system language '{Application.systemLanguage}'");
			ChangeLanguage(Application.systemLanguage);
		}
		
		private void LoadStrings()
		{
			LocalizedString[] strings = Resources.LoadAll<LocalizedString>(string.Empty);
			StringsMap = strings.ToDictionary(_ => _.Id, _ => _);
			Debug.Log($"Loc: {strings.Length} localized strings are loaded");
		}

		public void ChangeLanguage(SystemLanguage language)
		{
			Debug.Log($"Loc: Change language to '{language}'");
			Language = language;
			NotifyHandlers();
			NotifySceneComponents(Language);
		}

		private void NotifyHandlers()
		{
			foreach (ILocalized handler in Handlers)
			{
				try
				{
					handler.OnLocalizationChanged();
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}

		private static void NotifySceneComponents(SystemLanguage language)
		{
			foreach (LocalizedTextComponent text in Object.FindObjectsOfType<LocalizedTextComponent>(includeInactive: true))
				text.Localize(language);
		}

		public string Get(string term)
		{
			if (StringsMap.TryGetValue(term, out LocalizedString localizedString))
				return localizedString.GetText(Language);
			return $"mis:{term}";
		}

#if UNITY_EDITOR
		public static void ChangeLanguageFromEditor(SystemLanguage language)
		{
			NotifySceneComponents(language);
		}
#endif
	}
}