using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    private void Start()
    {
        ChangeLocale(PlayerPrefs.GetInt("LanguageID", 0));
    }

    public void ChangeLocale(int id)
    {
        StartCoroutine(SetLanguage(id));
    }

    IEnumerator SetLanguage(int languageID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
        PlayerPrefs.SetInt("LanguageID", languageID);
    }
}
