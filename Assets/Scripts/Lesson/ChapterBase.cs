using System;
using UnityEngine;
using UnityEngine.Playables;

public class ChapterBase : MonoBehaviour, IChapter
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private bool _hasInteractiveButton; // Manual "next" button

    public event Action OnChapterComplete;

    private void Start()
    {
        if (_timeline && !_hasInteractiveButton)
        {
            _timeline.stopped += OnTimelineComplete;
        }
    }

    private void OnTimelineComplete(PlayableDirector director)
    {
        OnChapterComplete?.Invoke();
    }

    public void OnNextButtonPressed()
    {
        OnChapterComplete?.Invoke();
    }

    private void OnDestroy()
    {
        if (_timeline != null) _timeline.stopped -= OnTimelineComplete;
    }
}
