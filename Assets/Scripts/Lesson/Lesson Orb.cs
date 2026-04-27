using System;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class LessonOrb : MonoBehaviour
{
    // System for displaying lessons
    // Restarts entire lesson when picked up again
    
    public static Action<int> OnLessonHide;
    public static Action<int> OnLessonComplete; // Affects pt giver and next lesson orb shows up

    [SerializeField] private string _lessonName;
    [SerializeField] private int _lessonNumber;

    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private Animator _animator; // Used to switch timeline states/slides

    private int _ownID;
    private bool hasCompleted;
    
    public Transform snapTarget; // Updated by snap location trigger
    public bool inLessonZone;
    
    private Grabbable _grabbable;
    private bool _grabEventListening;
    
    private void Start()
    {
        _ownID = gameObject.GetInstanceID();
        _animator.enabled = false;
        
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
                break;
        }
    }
    
    private void CheckLessonComplete(int id)
    {
        if (id == _ownID) hasCompleted = true;
    }

    public void StartLesson()
    {
        _animator.enabled = true;
    }
    
    private void HideLesson()
    {
        // If orb picked up the lesson is hidden
        // TODO: Maybe animator moves to a hidden state
        _animator.enabled = false;
    }
    
}
