using System;
using UnityEngine;

public class NextLessonTopic : MonoBehaviour
{
    public static Action<string> OnNext;

    [SerializeField] private string nextTopicID; // Used for hiding other topics
    [SerializeField] private GameObject nextTopicObj;
    

    [ContextMenu("NextTopic")]
    public void MoveToNextTopic()
    {
        OnNext?.Invoke(nextTopicID);
        nextTopicObj.SetActive(true); // Is there a better way for this?
    }
}
