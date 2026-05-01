
using System;
using UnityEngine;

public class LessonManager : MonoBehaviour
{
    public event Action OnLessonComplete;
    
    [SerializeField] private Transform _chapterSpawnLocation;

    private LessonConfig _currentLesson;
    private int _currentChapterIndex;
    
    
    private GameObject _activeChapterInstance;

    public void SetLesson(LessonConfig lesson, Transform chapterSpawnLocation)
    {
        _currentLesson = lesson;
        _chapterSpawnLocation = chapterSpawnLocation;
        LoadChapter(0);
    }
    
    public void LoadChapter(int chapterIndex)
    {
        if (chapterIndex == -1)
        {
            OnLessonComplete?.Invoke();
            return;
        }
        
        ChapterNode chapter = _currentLesson.GetChapter(chapterIndex);
        if (chapter == null)
        {
            Debug.Log("Lesson complete!");
            OnLessonComplete?.Invoke();
            return;
        }

        _currentChapterIndex = chapterIndex;

        // Removing last chapter
        if (_activeChapterInstance != null) Destroy(_activeChapterInstance);

        // Get prefab based on theme
        var prefab = chapter.GetPrefabForTheme(ThemeManager.Instance._themeChoice);
        _activeChapterInstance = Instantiate(prefab, _chapterSpawnLocation);

        // Register for timeline completion
        IChapter chapterComponent = _activeChapterInstance.GetComponent<IChapter>();
        if (chapterComponent != null)
        {
            chapterComponent.OnChapterComplete += GoToNextChapter;
        }

        Debug.Log($"Loaded Chapter {chapterIndex} ({ThemeManager.Instance._themeChoice})");
    }

    private void GoToNextChapter()
    {
        ChapterNode currentChapter = _currentLesson.GetChapter(_currentChapterIndex);
        int nextIndex = currentChapter.GetNextChapterIndex(ThemeManager.Instance._themeChoice);
        
        LoadChapter(nextIndex);
    }
}