using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _lessonSpace;
    [SerializeField] private Transform _quizSpace;
    
    public void TeleportPlayerToLessonSpace()
    {
        _player.transform.position = _lessonSpace.position;
    }
    
    public void TeleportPlayerToQuizSpace()
    {
        _player.transform.position = _quizSpace.position;
    }
}
