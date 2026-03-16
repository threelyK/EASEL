using UnityEngine;

namespace MCQ_UI
{
    [CreateAssetMenu(fileName = "MCQTemplate", menuName = "Scriptable Objects/UI/MCQ Template")]
    public class MCQTemplate : ScriptableObject
    {
        public string question;
        public OptionChoice answer;
        public string optionA;
        public string optionB;
        public string optionC;
        public string optionD;
    }
}
