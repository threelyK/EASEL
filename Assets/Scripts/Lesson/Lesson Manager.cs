
using System;
using UnityEngine;

public class LessonManager : MonoBehaviour
{
    public Action OnLessonComplete;
    
    [SerializeField] private Transform _chapterSpawnLocation;

    private LessonConfig _currentLesson;
    private int _currentChapterIndex;
    
    
    private GameObject _activeChapterInstance;
    private ThemeManager _themeManager;

    public void SetLesson(LessonConfig lesson, Transform chapterSpawnLocation)
    {
        _currentLesson = lesson;
        _chapterSpawnLocation = chapterSpawnLocation;
        LoadChapter(0);
    }
    
    public void LoadChapter(int chapterIndex)
    {
        ChapterNode chapter = _currentLesson.GetChapter(chapterIndex);
        if (chapter == null)
        {
            Debug.Log("Lesson complete!");
            return;
        }

        _currentChapterIndex = chapterIndex;

        // Removing last chapter
        if (_activeChapterInstance != null) Destroy(_activeChapterInstance);

        // Get prefab based on theme
        var prefab = chapter.GetPrefabForTheme(_themeManager._themeChoice);
        _activeChapterInstance = Instantiate(prefab, _chapterSpawnLocation);

        // Register for timeline completion
        IChapter chapterComponent = _activeChapterInstance.GetComponent<IChapter>();
        if (chapterComponent != null)
        {
            chapterComponent.OnChapterComplete += GoToNextChapter;
        }

        Debug.Log($"Loaded Chapter {chapterIndex} ({_themeManager.GetTheme()})");
    }

    private void GoToNextChapter()
    {
        ChapterNode currentChapter = _currentLesson.GetChapter(_currentChapterIndex);
        int nextIndex = currentChapter.GetNextChapterIndex(_themeManager._themeChoice);
        
        LoadChapter(nextIndex);
    }
}