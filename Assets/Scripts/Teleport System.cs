using UnityEngine;
using Oculus.Interaction.Locomotion;

public class TeleportSystem : MonoBehaviour
{
    // Used to teleport the player to specified locations
    
    [SerializeField] private FirstPersonLocomotor _playerLocomotor;

    [SerializeField] private bool _shouldUseStartingTransform;
    [SerializeField] private Transform _startingTransform;
    
    [SerializeField] private Transform _lessonSpace;
    [SerializeField] private Transform _quizSpace;

    private void Start()
    {
        if (_playerLocomotor == null) Debug.LogError("No PlayerLocomotor");

        if (_shouldUseStartingTransform)
        { 
            Invoke(nameof(TeleportToStart), 2f);
        }
    }
    
    private void TeleportToPosition(Vector3 targetPosition)
    {

        var translationEvent = new LocomotionEvent(0, targetPosition, LocomotionEvent.TranslationType.Absolute);
        
        _playerLocomotor.HandleLocomotionEvent(translationEvent);
    }

    public void TeleportAndRotateToPosition(Transform targetTransform)
    {

        var tpPose = new Pose(targetTransform.position, targetTransform.rotation);
        var locomotionEvent = new LocomotionEvent(identifier: 0, pose: tpPose,
            translationType: LocomotionEvent.TranslationType.Absolute,
            rotationType: LocomotionEvent.RotationType.Absolute);
        
        _playerLocomotor.HandleLocomotionEvent(locomotionEvent);
    }

    [ContextMenu("Teleport to Start")]
    public void TeleportToStart()
    {
        TeleportAndRotateToPosition(_startingTransform);
    }
    
    [ContextMenu("Teleport to Lesson Space")]
    public void TeleportToLessonSpace()
    {
        TeleportToPosition(_lessonSpace.position);
    }
    
    [ContextMenu("Teleport to Quiz Space")]
    public void TeleportToQuizSpace()
    {
        TeleportAndRotateToPosition(_quizSpace);
    }
}
