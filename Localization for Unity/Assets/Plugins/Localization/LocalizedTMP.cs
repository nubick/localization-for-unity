using TMPro;
using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMP : LocalizedTextComponent
    {
        private TMP_Text _textComponent;
        
        private TMP_Text GetTextComponent()
        {
            if(_textComponent == null)
                _textComponent = GetComponent<TMP_Text>();
            return _textComponent;
        }
        
        protected override void SetText(string localizedString)
        {
            GetTextComponent().text = localizedString;
        }
    }
}