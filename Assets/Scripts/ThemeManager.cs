using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public ThemeData lightTheme;
    public ThemeData darkTheme;
    public ThemeData currentTheme;

    public TextMeshProUGUI themeText;

    public Image[] backgrounds;
    public TextMeshProUGUI[] texts;
    public Button[] buttons;

    void Start()
    {
        ApplyTheme(lightTheme);
    }

    public void ToggleTheme()
    {
        ThemeData Theme = currentTheme == lightTheme ? darkTheme : lightTheme;
        ApplyTheme(Theme);
    }

    void ApplyTheme(ThemeData theme)
    {
        currentTheme = theme;

        themeText.text = theme == lightTheme ? darkTheme.ThemeName : lightTheme.ThemeName;

        foreach (Image bg in backgrounds)
        {
            bg.color = theme.backgroundColor;
        }

        foreach (TextMeshProUGUI text in texts)
        {
            text.color = theme.textColor;
        }

        foreach (Button button in buttons)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = theme.buttonColor;
            button.colors = cb;
            button.GetComponentInChildren<TextMeshProUGUI>().color = theme.buttonTextColor;
        }
    }
}
