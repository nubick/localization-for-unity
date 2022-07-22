using UnityEngine;

namespace Localization
{
	[CreateAssetMenu]
	public class LocalizedString : ScriptableObject
	{
		public string Id;
		[TextArea(5, 10)] public string English;
		[TextArea(5, 10)] public string Russian;

		public string GetText(SystemLanguage language)
		{
			if (language == SystemLanguage.Russian)
				return Russian;
			else
				return English;
		}

		public override string ToString() => $"[{Id}|{English}|{Russian}]";
	}
}