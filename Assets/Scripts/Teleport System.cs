using UnityEngine;
using Oculus.Interaction.Locomotion;

public class TeleportSystem : MonoBehaviour
{
    [SerializeField] private GameObject _playerRig;
    [SerializeField] private GameObject _playerController;

    [SerializeField] private FirstPersonLocomotor _fpLocomotor;
    
    [SerializeField] private Transform _lessonSpace;
    [SerializeField] private Transform _quizSpace;

    private void Start()
    {
        _fpLocomotor = FindFirstObjectByType<FirstPersonLocomotor>();
        if (_fpLocomotor == null) Debug.LogError("No PlayerLocomotor");
    }
    
    [ContextMenu("Teleport to Lesson Space")]
    public void TeleportPlayerToLessonSpace()
    {
        _playerRig.transform.position = _lessonSpace.position;
        _playerController.transform.position = _lessonSpace.position;
    }
    
    [ContextMenu("Teleport to Quiz Space")]
    public void TeleportPlayerToQuizSpace()
    {
        _playerRig.transform.position = _quizSpace.position;
        _playerController.transform.position = _quizSpace.position;
    }
}
