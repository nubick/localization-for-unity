using UnityEngine;

namespace Localization
{
    public abstract class LocalizedTextComponent : MonoBehaviour
    {
        [SerializeField] private LocalizedString LocalizedString;
        
        protected abstract void SetText(string localizedString);
        
        public void Localize(SystemLanguage language)
        {
            if (LocalizedString == null)
            {
                Debug.LogError($"LocalizedString is null for '{name}'");
                return;
            }

            string localizedString = LocalizedString.GetText(language);
            SetText(localizedString);
        }
    }
}