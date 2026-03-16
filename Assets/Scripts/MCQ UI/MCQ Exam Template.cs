using UnityEngine;

namespace MCQ_UI
{
    [CreateAssetMenu(fileName = "MCQExamTemplate", menuName = "Scriptable Objects/UI/MCQ Exam Template")]
    public class MCQExamTemplate : ScriptableObject
    {
        public string ExamID;
        public MCQTemplate[] questions;
    }
}
