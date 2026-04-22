using System;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    // Should only update dialogue systems once at the beginning
    public static Action<ThemeType> OnThemeChange;
    [SerializeField] private bool _isEnabled = true;
    

    private ThemeType _themeChoice;

    public ThemeType GetTheme()
    {
        return _themeChoice;
    }

    public void ChangeThemeToFood()
    {
        _themeChoice = ThemeType.Food;
        OnThemeChange?.Invoke(ThemeType.Food);
        Debug.Log("Theme changed to food");
    }

    public void ChangeThemeToGames()
    {
        _themeChoice = ThemeType.Games;
        OnThemeChange?.Invoke(ThemeType.Games);
        Debug.Log("Theme changed to games");
    }
}

public enum ThemeType
{
    Food,
    Games
}
