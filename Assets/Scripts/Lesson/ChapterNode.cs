using System;
using UnityEngine;

[Serializable]
public class ChapterNode
{
    [SerializeField] private int _chapterNumber;
    [SerializeField] private GameObject _foodThemePrefab;
    [SerializeField] private GameObject _gamesThemePrefab;
    [SerializeField] private bool _isGeneric; // Defaults to foodThemePrefab if generic
    [SerializeField] private int _nextChapterIndexFood = -1; // -1 means lesson ends
    [SerializeField] private int _nextChapterIndexGames = -1;

    public GameObject GetPrefabForTheme(ThemeType theme)
    {
        if (_isGeneric)
            return _foodThemePrefab;

        return theme switch
        {
            ThemeType.Food => _foodThemePrefab,
            ThemeType.Games => _gamesThemePrefab,
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null)
        };
    }

    public int GetNextChapterIndex(ThemeType theme)
    {
        return theme switch
        {
            ThemeType.Food => _nextChapterIndexFood,
            ThemeType.Games => _nextChapterIndexGames,
            _ => -1
        };
    }
}