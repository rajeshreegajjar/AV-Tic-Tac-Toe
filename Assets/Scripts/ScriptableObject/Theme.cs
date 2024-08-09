using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "ScriptableObjects/Theme", order = 1)]
public class ThemeData : ScriptableObject
{
    public string ThemeName;
    public Color backgroundColor;
    public Color textColor;
    public Color buttonColor;
    public Color buttonTextColor;
}
