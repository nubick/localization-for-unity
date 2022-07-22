using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
	[RequireComponent(typeof(Text))]
	public class LocalizedText : LocalizedTextComponent
	{
		private Text _textComponent;
		
		private Text GetTextComponent()
		{
			if (_textComponent == null)
				_textComponent = GetComponent<Text>();
			return _textComponent;
		}
		
		protected override void SetText(string localizedString)
		{
			GetTextComponent().text = localizedString;
			LayoutRebuilder.ForceRebuildLayoutImmediate(_textComponent.rectTransform);
		}
	}
}