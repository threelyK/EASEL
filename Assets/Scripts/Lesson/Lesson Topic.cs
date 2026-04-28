using UnityEngine;

public class LessonTopic : MonoBehaviour
{
    [SerializeField] private string ownTopicID;
    
    private void OnEnable()
    {
        NextLessonTopic.OnNext += HideTopic;
    }

    private void HideTopic(string targetID)
    {
        if (targetID != ownTopicID) gameObject.SetActive(false);
    }
    
}
