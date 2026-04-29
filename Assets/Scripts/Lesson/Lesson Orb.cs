using System;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class LessonOrb : MonoBehaviour
{
    // System for displaying lessons
    // Restarts entire lesson when picked up again
    
    public static Action<int> OnLessonHide;
    public static Action<int> OnLessonComplete; // Affects point giver and next lesson orb shows up
    
    [SerializeField] private LessonConfig _lessonConfig;
    [SerializeField] private string _lessonName;
    [SerializeField] private int _lessonNumber;

    [SerializeField] private TMP_Text _infoText;

    private int _ownID;
    private bool _hasCompleted;
    
    public Transform snapTarget; // Updated by snap location trigger
    public bool inLessonZone;
    
    private Grabbable _grabbable;
    private bool _grabEventListening;
    
    private LessonManager _activeLessonManager;
    
    private void Start()
    {
        _ownID = gameObject.GetInstanceID();
        
        _grabbable = GetComponent<Grabbable>();

        if (!_grabEventListening) _grabbable.WhenPointerEventRaised += HandleGrabbableEvent;
    }

    private void OnEnable()
    {
        OnLessonComplete += CheckLessonComplete;
        
        if (!_grabEventListening && _grabbable)
        {
            _grabbable.WhenPointerEventRaised += HandleGrabbableEvent;
            _grabEventListening = true;
        }
    }
    
    private void OnDisable()
    {
        _grabbable.WhenPointerEventRaised -= HandleGrabbableEvent;
        _grabEventListening = false;
        
        if (_activeLessonManager != null) Destroy(_activeLessonManager.gameObject);
    }

    private void HandleGrabbableEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Unselect)
        {
            
        }

        switch (evt.Type)
        {
            case PointerEventType.Unselect:
                // Snap to snapTarget 
                if (snapTarget is not null) transform.SetPositionAndRotation(snapTarget.position, snapTarget.rotation);
            
                // Play lesson
                if (inLessonZone) StartLesson();
                break;
            
            case PointerEventType.Select:
                OnLessonHide?.Invoke(_ownID);
                HideLesson();
                break;
        }
    }
    
    private void CheckLessonComplete(int id)
    {
        if (id == _ownID) _hasCompleted = true;
    }

    public void StartLesson()
    {
        if (_activeLessonManager != null || !inLessonZone) return;
        
        // Creates lesson manager for this lesson
        var lessonManagerObj = new GameObject($"LessonManager_{_lessonNumber}");
        _activeLessonManager = lessonManagerObj.AddComponent<LessonManager>();

        _activeLessonManager.SetLesson(_lessonConfig);
        _activeLessonManager.OnLessonComplete += HandleLessonComplete;
    }
    
    private void HideLesson()
    {
        if (_activeLessonManager != null)
        {
            _activeLessonManager.OnLessonComplete -= HandleLessonComplete;
            Destroy(_activeLessonManager.gameObject);
            _activeLessonManager = null;
        }
    }

    private void HandleLessonComplete()
    {
        OnLessonComplete?.Invoke(_ownID);
        HideLesson();
    }
    
}
