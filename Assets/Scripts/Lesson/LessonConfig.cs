using UnityEngine;

[CreateAssetMenu(fileName = "Lesson", menuName = "Scriptable Objects/Lesson/LessonConfig")]
public class LessonConfig : ScriptableObject
{
    [SerializeField] private int _lessonNumber;
    [SerializeField] private ChapterNode[] _chapters;

    public ChapterNode GetChapter(int index)
    {
        return index < _chapters.Length ? _chapters[index] : null;
    }

    public int ChapterCount => _chapters.Length;
}
