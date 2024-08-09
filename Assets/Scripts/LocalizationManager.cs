using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    public TextMeshProUGUI languageText;

    public TMP_FontAsset englishFont, hindiFont;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        Debug.Log("LocalizationManager");
        //AsyncOperationHandle<LocalizationSettings> localeOp = LocalizationSettings.InitializationOperation;
        //if (!localeOp.IsDone)
        //    localeOp.Completed += InitDone;
        //else
        //    InitDone(localeOp);



        SetLanguage("en");
    }

    private void InitDone(AsyncOperationHandle<LocalizationSettings> handle)
    {
        Debug.Log("InitDone");
        Locale locale = LocalizationSettings.SelectedLocale;
        Debug.Log(locale.Identifier.Code);
        SetLanguage(locale.Identifier.Code);
    }

    public void SetLanguage(string localeCode)
    {
        switch (localeCode)
        {
            case "en":
                languageText.text = "Hindi";
                break;
            case "hi":
                languageText.text = "English";
                break;
            default:
                break;
        }
        Locale selectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        LocalizationSettings.SelectedLocale = selectedLocale;
        Debug.Log("SetLanguage" + localeCode);
    }

    public void ToggleLanguage()
    {
        string currentLocale = LocalizationSettings.SelectedLocale.Identifier.Code;
        string newLocale = currentLocale == "en" ? "hi" : "en";
        SetLanguage(newLocale);
    }


}
