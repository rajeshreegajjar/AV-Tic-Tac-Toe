using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizedStringWithEvents : MonoBehaviour
{
    public LocalizedString myString;
    public Locale locale;
    private string value;

    public TextMeshProUGUI localizedText;


    /// <summary>
    /// Register a ChangeHandler. This is called whenever the string needs to be updated.
    /// </summary>
    void OnEnable()
    {

        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        myString.StringChanged += UpdateString;
    }

    void OnDisable()
    {

        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
        myString.StringChanged -= UpdateString;
    }

    private void LocalizationSettings_SelectedLocaleChanged(Locale obj)
    {
        Debug.Log("LocalizationSettings_SelectedLocaleChanged");
        locale = obj;
        setText();
    }



    void UpdateString(string s)
    {
        Debug.Log("UpdateString");
        if (locale == null)
            return;

        value = s;
        setText();
    }


    void setText()
    {
        switch (locale.Identifier.Code)
        {
            case "en":
                localizedText.font = LocalizationManager.Instance.englishFont;
                if (value != null) localizedText.text = value;
                break;
            case "hi":
                localizedText.font = LocalizationManager.Instance.hindiFont;
                if (value != null) localizedText.text = UnicodeToKrutidev.UnicodeToKrutiDev(value); ;
                break;
            default:
                break;
        }
    }
}
